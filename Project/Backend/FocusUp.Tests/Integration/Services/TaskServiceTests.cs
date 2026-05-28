using FluentAssertions;
using Xunit;
using FocusUp.Application.Services;
using FocusUp.Domain.Enums;
using FocusUp.Infrastructure.Repositories;

namespace FocusUp.Tests.Services;

/// <summary>
/// Contains unit tests for the TaskService.
/// Tests creating, validating, updating, deleting,
/// and retrieving tasks by user ID.
/// </summary>
public class TaskServiceTests
{
    /// <summary>
    /// Tests whether a valid task is created
    /// and saved correctly in the database.
    /// </summary>
    [Fact]
    public void CreateTask_WithValidTask_ShouldCreateTask()
    {
        var db = TestDatabaseHelper.Db;
        var userId = TestDatabaseHelper.InsertUser();

        var taskRepository = new TaskRepository(db);
        var userStatsRepository = new UserStatsRepository(db);
        userStatsRepository.Insert(new UserStats(userId));

        var service = new TaskService(taskRepository, userStatsRepository);

        var task = new Task(
            userId,
            "Task",
            "Desc",
            TaskDifficultyType.Easy,
            30,
            FocusUp.Domain.Enums.TaskStatus.Open
        );

        var taskId = service.CreateTask(task);

        var createdTask = taskRepository.GetById(taskId);
        createdTask.Should().NotBeNull();
        createdTask!.Title.Should().Be("Task");
    }

    /// <summary>
    /// Tests whether an invalid task fails model validation.
    /// </summary>
    [Fact]
    public void CreateTask_WithInvalidTask_ShouldBeInvalidByModelValidation()
    {
        var task = new Task(
            1,
            "",
            "Desc",
            TaskDifficultyType.Easy,
            30,
            FocusUp.Domain.Enums.TaskStatus.Open
        );

        task.ValidateData().Should().BeFalse();
    }

    /// <summary>
    /// Tests whether task updates are saved correctly.
    /// </summary>
    [Fact]
    public void UpdateTask_ShouldUpdateTask()
    {
        var db = TestDatabaseHelper.Db;
        var userId = TestDatabaseHelper.InsertUser();

        var taskRepository = new TaskRepository(db);
        var userStatsRepository = new UserStatsRepository(db);
        userStatsRepository.Insert(new UserStats(userId));

        var service = new TaskService(taskRepository, userStatsRepository);

        var task = new Task(
            userId,
            "Old",
            "Desc",
            TaskDifficultyType.Easy,
            30,
            FocusUp.Domain.Enums.TaskStatus.Open
        );

        var taskId = taskRepository.Insert(task);

        var updatedTask = new Task(
            taskId,
            userId,
            "New",
            "Updated",
            TaskDifficultyType.Hard,
            60,
            FocusUp.Domain.Enums.TaskStatus.Open
        );

        service.UpdateTask(updatedTask);

        var result = taskRepository.GetById(taskId);
        result!.Title.Should().Be("New");
        result.Difficulty.Should().Be(TaskDifficultyType.Hard);
    }

    /// <summary>
    /// Tests whether a task can be deleted successfully.
    /// </summary>
    [Fact]
    public void DeleteTask_ShouldDeleteTask()
    {
        var db = TestDatabaseHelper.Db;
        var userId = TestDatabaseHelper.InsertUser();

        var taskRepository = new TaskRepository(db);
        var userStatsRepository = new UserStatsRepository(db);
        userStatsRepository.Insert(new UserStats(userId));

        var service = new TaskService(taskRepository, userStatsRepository);

        var task = new Task(
            userId,
            "Task",
            "Desc",
            TaskDifficultyType.Easy,
            30,
            FocusUp.Domain.Enums.TaskStatus.Open
        );

        var taskId = taskRepository.Insert(task);

        service.DeleteTask(taskId);

        taskRepository.GetById(taskId).Should().BeNull();
    }

    /// <summary>
    /// Tests whether only tasks belonging to the requested user
    /// are returned by GetTaskByUserId.
    /// </summary>
    [Fact]
    public void GetTaskByUserId_ShouldOnlyReturnTasksOfCorrectUser()
    {
        var db = TestDatabaseHelper.Db;
        var userId = TestDatabaseHelper.InsertUser();

        var connection = db.GetConnection();
        using var cmd = connection.CreateCommand();
        cmd.CommandText = """
        INSERT INTO User (username, email, password_hash)
        VALUES ('otheruser', 'other@example.com', 'hash');
        SELECT last_insert_rowid();
        """;
        var otherUserId = Convert.ToInt32(cmd.ExecuteScalar());

        var taskRepository = new TaskRepository(db);
        var userStatsRepository = new UserStatsRepository(db);

        taskRepository.Insert(new Task(
            userId,
            "User Task",
            "Desc",
            TaskDifficultyType.Easy,
            30,
            FocusUp.Domain.Enums.TaskStatus.Open
        ));

        taskRepository.Insert(new Task(
            otherUserId,
            "Other Task",
            "Desc",
            TaskDifficultyType.Easy,
            30,
            FocusUp.Domain.Enums.TaskStatus.Open
        ));

        var service = new TaskService(taskRepository, userStatsRepository);

        var result = service.GetTaskByUserId(userId);

        result.Should().OnlyContain(t => t.UserId == userId);
    }
}