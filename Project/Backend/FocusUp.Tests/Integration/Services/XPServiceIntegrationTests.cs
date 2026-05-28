using FluentAssertions;
using Xunit;
using FocusUp.Application.Services;
using FocusUp.Application.Strategies;
using FocusUp.Domain.Enums;
using FocusUp.Infrastructure.Repositories;

namespace FocusUp.Tests.Integration.Services;

/// <summary>
/// Contains integration tests for the XPService.
/// Tests XP event creation, XP total updates,
/// and checking whether XP events already exist.
/// </summary>
public class XPServiceIntegrationTests
{
    /// <summary>
    /// Tests whether awarding XP creates one XP event
    /// with the expected XP amount.
    /// </summary>
    [Fact]
    public void AwardXP_ShouldCreateXpEvent()
    {
        var db = TestDatabaseHelper.Db;
        var userId = TestDatabaseHelper.InsertUser();

        var userStatsRepo = new UserStatsRepository(db);
        var xpEventRepo = new XPEventRepository(db);
        var taskRepo = new TaskRepository(db);

        userStatsRepo.Insert(new UserStats(userId));

        var taskId = taskRepo.Insert(new Task(
            userId,
            "Task",
            "Desc",
            TaskDifficultyType.Easy,
            30,
            FocusUp.Domain.Enums.TaskStatus.Open
        ));

        var task = new Task(
            taskId,
            userId,
            "Task",
            "Desc",
            TaskDifficultyType.Easy,
            30,
            FocusUp.Domain.Enums.TaskStatus.Open
        );

        var service = new XPService(
            xpEventRepo,
            userStatsRepo,
            new DefaultXpCalculationStrategy()
        );

        service.AwardXP(userId, task, 50, RewardReason.TaskCompleted);

        var events = xpEventRepo.GetAllByTaskId(taskId);

        events.Should().HaveCount(1);
        events[0].Amount.Should().Be(50);
    }

    /// <summary>
    /// Tests whether awarding XP increases
    /// the user's total XP value.
    /// </summary>
    [Fact]
    public void AwardXP_ShouldIncreaseTotalXp()
    {
        var db = TestDatabaseHelper.Db;
        var userId = TestDatabaseHelper.InsertUser();

        var userStatsRepo = new UserStatsRepository(db);
        var xpEventRepo = new XPEventRepository(db);
        var taskRepo = new TaskRepository(db);

        userStatsRepo.Insert(new UserStats(userId));

        var taskId = taskRepo.Insert(new Task(
            userId,
            "Task",
            "Desc",
            TaskDifficultyType.Easy,
            30,
            FocusUp.Domain.Enums.TaskStatus.Open
        ));

        var task = new Task(
            taskId,
            userId,
            "Task",
            "Desc",
            TaskDifficultyType.Easy,
            30,
            FocusUp.Domain.Enums.TaskStatus.Open
        );

        var service = new XPService(
            xpEventRepo,
            userStatsRepo,
            new DefaultXpCalculationStrategy()
        );

        service.AwardXP(userId, task, 50, RewardReason.TaskCompleted);

        var stats = userStatsRepo.GetByUserId(userId);

        stats!.TotalXp.Should().Be(50);
    }

    /// <summary>
    /// Tests whether HasXPEventForTask returns true
    /// when an XP event already exists for a task.
    /// </summary>
    [Fact]
    public void HasXPEventForTask_WhenEventExists_ShouldReturnTrue()
    {
        var db = TestDatabaseHelper.Db;
        var userId = TestDatabaseHelper.InsertUser();

        var userStatsRepo = new UserStatsRepository(db);
        var xpEventRepo = new XPEventRepository(db);
        var taskRepo = new TaskRepository(db);

        userStatsRepo.Insert(new UserStats(userId));

        var taskId = taskRepo.Insert(new Task(
            userId,
            "Task",
            "Desc",
            TaskDifficultyType.Easy,
            30,
            FocusUp.Domain.Enums.TaskStatus.Open
        ));

        xpEventRepo.Insert(new XpEvent(
            userId,
            50,
            RewardReason.TaskCompleted,
            taskId
        ));

        var service = new XPService(
            xpEventRepo,
            userStatsRepo,
            new DefaultXpCalculationStrategy()
        );

        var result = service.HasXPEventForTask(taskId, RewardReason.TaskCompleted);

        result.Should().BeTrue();
    }
}