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

public class TasksControllerTests
{
    [Fact]
    public void GetTaskById_WithoutUserClaim_ShouldReturnUnauthorized()
    {
        var controller = new TasksController(null!, null!, null!, null!);
        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext()
        };

        var result = controller.GetTaskById(1);

        result.Should().BeOfType<UnauthorizedResult>();
    }

    [Fact]
    public void GetTaskById_WhenTaskDoesNotExist_ShouldReturnNotFound()
    {
        var db = TestDatabaseHelper.Db;
        var userId = TestDatabaseHelper.InsertUser();

        var controller = CreateController(db, userId);
        SetUser(controller, userId);

        var result = controller.GetTaskById(999);

        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public void GetTaskById_WhenTaskBelongsToOtherUser_ShouldReturnForbidden()
    {
        var db = TestDatabaseHelper.Db;

        var ownerId = TestDatabaseHelper.InsertUser();
        var otherUserId = TestDatabaseHelper.InsertUser();

        var userStatsRepo = new UserStatsRepository(db);
        userStatsRepo.Insert(new UserStats(ownerId));
        userStatsRepo.Insert(new UserStats(otherUserId));

        var taskRepo = new TaskRepository(db);
        var taskId = taskRepo.Insert(new Task(
            ownerId,
            "Task",
            "Desc",
            TaskDifficultyType.Easy,
            30,
            FocusUp.Domain.Enums.TaskStatus.Open
        ));

        var controller = CreateController(db, otherUserId);
        SetUser(controller, otherUserId);

        var result = controller.GetTaskById(taskId);

        result.Should().BeOfType<ForbidResult>();
    }

    [Fact]
    public void GetTaskById_WithValidTask_ShouldReturnExpectedResponseStructure()
    {
        var db = TestDatabaseHelper.Db;
        var userId = TestDatabaseHelper.InsertUser();

        var userStatsRepo = new UserStatsRepository(db);
        userStatsRepo.Insert(new UserStats(userId));

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

        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        var dto = okResult.Value.Should().BeOfType<TaskDto>().Subject;

        dto.Id.Should().Be(taskId);
        dto.UserId.Should().Be(userId);
        dto.Title.Should().Be("Task");
        dto.Description.Should().Be("Desc");
        dto.Difficulty.Should().Be(TaskDifficultyType.Easy);
        dto.DurationMin.Should().Be(30);
    }

    [Fact]
    public void CreateTask_WithInvalidInput_ShouldReturnBadRequest()
    {
        var db = TestDatabaseHelper.Db;
        var userId = TestDatabaseHelper.InsertUser();

        var controller = CreateController(db, userId);
        SetUser(controller, userId);

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

    [Fact]
    public void CreateTask_WithValidInput_ShouldReturnCreatedStatusCode()
    {
        var db = TestDatabaseHelper.Db;
        var userId = TestDatabaseHelper.InsertUser();

        var userStatsRepo = new UserStatsRepository(db);
        userStatsRepo.Insert(new UserStats(userId));

        var controller = CreateController(db, userId);
        SetUser(controller, userId);

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

    private static TasksController CreateController(DatabaseConnection db, int userId)
    {
        var taskRepo = new TaskRepository(db);
        var userStatsRepo = new UserStatsRepository(db);
        var xpEventRepo = new XPEventRepository(db);
        var taskLogRepo = new TaskLogRepository(db);
        var badgeRepo = new BadgeRepository(db);
        var userBadgeRepo = new UserBadgeRepository(db);

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

        return new TasksController(
            taskCompletionService,
            taskService,
            xpService,
            userStatsService
        );
    }

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