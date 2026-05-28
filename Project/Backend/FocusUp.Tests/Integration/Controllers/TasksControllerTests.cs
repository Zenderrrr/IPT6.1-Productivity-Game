using FluentAssertions;
using Xunit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using FocusUp.Application.DTOs;
using FocusUp.Application.Services;
using FocusUp.Application.Strategies;
using FocusUp.Controllers;
using FocusUp.Domain.Enums;
using FocusUp.Infrastructure.Repositories;

namespace FocusUp.Tests.Controllers;

/// <summary>
/// Unit tests for the TasksController.
/// Tests authorization, validation, task ownership,
/// and successful task operations.
/// </summary>
public class TasksControllerTests
{
    /// <summary>
    /// Ensures unauthorized users cannot access a task.
    /// </summary>
    [Fact]
    public void GetTaskById_WithoutUserClaim_ShouldReturnUnauthorized()
    {
        // Create controller without authenticated user
        var controller = new TasksController(null!, null!, null!, null!);
        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext()
        };

        // Execute request
        var result = controller.GetTaskById(1);

        // Verify unauthorized response
        result.Should().BeOfType<UnauthorizedResult>();
    }

    /// <summary>
    /// Ensures requesting a non-existing task returns NotFound.
    /// </summary>
    [Fact]
    public void GetTaskById_WhenTaskDoesNotExist_ShouldReturnNotFound()
    {
        var db = TestDatabaseHelper.Db;
        var userId = TestDatabaseHelper.InsertUser();

        var controller = CreateController(db, userId);
        SetUser(controller, userId);

        // Request invalid task id
        var result = controller.GetTaskById(999);

        result.Should().BeOfType<NotFoundResult>();
    }

    /// <summary>
    /// Ensures users cannot access tasks belonging to another user.
    /// </summary>
    [Fact]
    public void GetTaskById_WhenTaskBelongsToOtherUser_ShouldReturnForbidden()
    {
        var db = TestDatabaseHelper.Db;

        var ownerId = TestDatabaseHelper.InsertUser();
        var otherUserId = TestDatabaseHelper.InsertUser();

        // Insert user statistics
        var userStatsRepo = new UserStatsRepository(db);
        userStatsRepo.Insert(new UserStats(ownerId));
        userStatsRepo.Insert(new UserStats(otherUserId));

        // Create task for owner
        var taskRepo = new TaskRepository(db);
        var taskId = taskRepo.Insert(new Task(
            ownerId,
            "Task",
            "Desc",
            TaskDifficultyType.Easy,
            30,
            FocusUp.Domain.Enums.TaskStatus.Open
        ));

        // Authenticate as different user
        var controller = CreateController(db, otherUserId);
        SetUser(controller, otherUserId);

        var result = controller.GetTaskById(taskId);

        result.Should().BeOfType<ForbidResult>();
    }

    /// <summary>
    /// Ensures valid tasks return correct DTO data.
    /// </summary>
    [Fact]
    public void GetTaskById_WithValidTask_ShouldReturnExpectedResponseStructure()
    {
        var db = TestDatabaseHelper.Db;
        var userId = TestDatabaseHelper.InsertUser();

        var userStatsRepo = new UserStatsRepository(db);
        userStatsRepo.Insert(new UserStats(userId));

        // Create valid task
        var taskRepo = new TaskRepository(db);
        var taskId = taskRepo.Insert(new Task(
            userId,
            "Task",
            "Desc",
            TaskDifficultyType.Easy,
            30,
            FocusUp.Domain.Enums.TaskStatus.Open
        ));

        var controller = CreateController(db, userId);
        SetUser(controller, userId);

        var result = controller.GetTaskById(taskId);

        // Validate response structure
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        var dto = okResult.Value.Should().BeOfType<TaskDto>().Subject;

        dto.Id.Should().Be(taskId);
        dto.UserId.Should().Be(userId);
        dto.Title.Should().Be("Task");
        dto.Description.Should().Be("Desc");
        dto.Difficulty.Should().Be(TaskDifficultyType.Easy);
        dto.DurationMin.Should().Be(30);
    }

    /// <summary>
    /// Ensures invalid task difficulty returns BadRequest.
    /// </summary>
    [Fact]
    public void CreateTask_WithInvalidInput_ShouldReturnBadRequest()
    {
        var db = TestDatabaseHelper.Db;
        var userId = TestDatabaseHelper.InsertUser();

        var controller = CreateController(db, userId);
        SetUser(controller, userId);

        // Invalid difficulty string
        var request = new CreateTaskRequest
        {
            Title = "Task",
            Description = "Desc",
            Difficulty = "InvalidDifficulty",
            DurationMin = 30
        };

        var result = controller.CreateTask(request);

        result.Should().BeOfType<BadRequestObjectResult>();
    }

    /// <summary>
    /// Ensures valid task creation returns HTTP 201.
    /// </summary>
    [Fact]
    public void CreateTask_WithValidInput_ShouldReturnCreatedStatusCode()
    {
        var db = TestDatabaseHelper.Db;
        var userId = TestDatabaseHelper.InsertUser();

        var userStatsRepo = new UserStatsRepository(db);
        userStatsRepo.Insert(new UserStats(userId));

        var controller = CreateController(db, userId);
        SetUser(controller, userId);

        // Valid task request
        var request = new CreateTaskRequest
        {
            Title = "Task",
            Description = "Desc",
            Difficulty = "Easy",
            DurationMin = 30
        };

        var result = controller.CreateTask(request);

        var statusResult = result.Should().BeOfType<ObjectResult>().Subject;
        statusResult.StatusCode.Should().Be(201);
    }

    /// <summary>
    /// Ensures deleting a non-existing task returns NotFound.
    /// </summary>
    [Fact]
    public void DeleteTask_WhenTaskDoesNotExist_ShouldReturnNotFound()
    {
        var db = TestDatabaseHelper.Db;
        var userId = TestDatabaseHelper.InsertUser();

        var controller = CreateController(db, userId);
        SetUser(controller, userId);

        var result = controller.DeleteTask(999);

        result.Should().BeOfType<NotFoundResult>();
    }

    /// <summary>
    /// Creates a fully initialized TasksController instance for testing.
    /// </summary>
    private static TasksController CreateController(DatabaseConnection db, int userId)
    {
        // Initialize repositories
        var taskRepo = new TaskRepository(db);
        var userStatsRepo = new UserStatsRepository(db);
        var xpEventRepo = new XPEventRepository(db);
        var taskLogRepo = new TaskLogRepository(db);
        var badgeRepo = new BadgeRepository(db);
        var userBadgeRepo = new UserBadgeRepository(db);

        // Initialize services
        var xpService = new XPService(
            xpEventRepo,
            userStatsRepo,
            new DefaultXpCalculationStrategy()
        );

        var taskService = new TaskService(
            taskRepo,
            userStatsRepo
        );

        var userStatsService = new UserStatsService(userStatsRepo);

        var streakService = new StreakService(
            userStatsRepo,
            new DefaultStreakRuleStrategy()
        );

        var badgeService = new BadgeService(
            userStatsRepo,
            badgeRepo,
            userBadgeRepo,
            [
                new TotalXpBadgeRule(),
                new TasksCompletedBadgeRule(),
                new TimeLoggedBadgeRule(),
                new StreakBadgeRule()
            ]
        );

        var taskCompletionService = new TaskCompletionService(
            taskRepo,
            xpService,
            streakService,
            null!,
            badgeService,
            userStatsRepo,
            xpEventRepo,
            taskLogRepo,
            db
        );

        // Return configured controller
        return new TasksController(
            taskCompletionService,
            taskService,
            xpService,
            userStatsService
        );
    }

    /// <summary>
    /// Sets authenticated test user into controller context.
    /// </summary>
    private static void SetUser(ControllerBase controller, int userId)
    {
        var identity = new ClaimsIdentity(
            [
                new Claim(ClaimTypes.NameIdentifier, userId.ToString())
            ],
            "TestAuth"
        );

        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(identity)
            }
        };
    }
}
