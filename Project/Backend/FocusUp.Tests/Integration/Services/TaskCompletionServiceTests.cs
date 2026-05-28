using FluentAssertions;
using Xunit;
using FocusUp.Application.Services;
using FocusUp.Application.Strategies;
using FocusUp.Domain.Enums;
using FocusUp.Infrastructure.Repositories;
using FocusUp.Common.Exceptions;

namespace FocusUp.Tests.Services;

/// <summary>
/// Contains unit tests for the TaskCompletionService.
/// Tests task completion, XP rewards, streak updates,
/// badge checks and task log creation.
/// </summary>
public class TaskCompletionServiceTests : IDisposable
{
    /// <summary>
    /// Shared database connection used by the tests.
    /// </summary>
    private readonly DatabaseConnection _db;

    /// <summary>
    /// Repository used to manage task data.
    /// </summary>
    private readonly TaskRepository _taskRepository;

    /// <summary>
    /// Repository used to manage user statistics.
    /// </summary>
    private readonly UserStatsRepository _userStatsRepository;

    /// <summary>
    /// Repository used to manage XP event records.
    /// </summary>
    private readonly XPEventRepository _xpEventRepository;

    /// <summary>
    /// Repository used to manage task log records.
    /// </summary>
    private readonly TaskLogRepository _taskLogRepository;

    /// <summary>
    /// Repository used to manage badge definitions.
    /// </summary>
    private readonly BadgeRepository _badgeRepository;

    /// <summary>
    /// Repository used to manage badges awarded to users.
    /// </summary>
    private readonly UserBadgeRepository _userBadgeRepository;

    /// <summary>
    /// Service under test.
    /// </summary>
    private readonly TaskCompletionService _taskCompletionService;

    /// <summary>
    /// Initializes repositories and services required for each test.
    /// </summary>
    public TaskCompletionServiceTests()
    {
        _db = TestDatabaseHelper.Db;

        _taskRepository = new TaskRepository(_db);
        _userStatsRepository = new UserStatsRepository(_db);
        _xpEventRepository = new XPEventRepository(_db);
        _taskLogRepository = new TaskLogRepository(_db);
        _badgeRepository = new BadgeRepository(_db);
        _userBadgeRepository = new UserBadgeRepository(_db);

        var xpService = new XPService(
            _xpEventRepository,
            _userStatsRepository,
            new DefaultXpCalculationStrategy()
        );

        var streakService = new StreakService(
            _userStatsRepository,
            new DefaultStreakRuleStrategy()
        );

        LevelService levelService = null!;

        var badgeService = new BadgeService(
            _userStatsRepository,
            _badgeRepository,
            _userBadgeRepository,
            [
                new TotalXpBadgeRule(),
                new TasksCompletedBadgeRule(),
                new TimeLoggedBadgeRule(),
                new StreakBadgeRule()
            ]
        );

        _taskCompletionService = new TaskCompletionService(
            _taskRepository,
            xpService,
            streakService,
            levelService,
            badgeService,
            _userStatsRepository,
            _xpEventRepository,
            _taskLogRepository,
            _db
        );
    }

    /// <summary>
    /// Cleans the test database after each test.
    /// </summary>
    public void Dispose()
    {
        TestDatabaseHelper.Clean();
    }

    /// <summary>
    /// Tests whether completing an open task changes
    /// its status to completed and sets the completion date.
    /// </summary>
    [Fact]
    public void CompleteTask_ShouldSetTaskToCompleted()
    {
        var userId = TestDatabaseHelper.InsertUser();

        _userStatsRepository.Insert(new UserStats(userId));

        var taskId = _taskRepository.Insert(
            new Task(
                userId,
                "Task",
                "Desc",
                TaskDifficultyType.Easy,
                30,
                FocusUp.Domain.Enums.TaskStatus.Open
            )
        );

        _taskCompletionService.CompleteTask(taskId, userId);

        var updatedTask = _taskRepository.GetById(taskId);

        updatedTask.Should().NotBeNull();
        updatedTask!.Status.Should().Be(FocusUp.Domain.Enums.TaskStatus.Completed);
        updatedTask.CompletedAt.Should().NotBeNull();
    }

    /// <summary>
    /// Tests whether completing an already completed task
    /// throws a TaskAlreadyCompletedException.
    /// </summary>
    [Fact]
    public void CompleteTask_AlreadyCompletedTask_ShouldThrowException()
    {
        var userId = TestDatabaseHelper.InsertUser();

        _userStatsRepository.Insert(new UserStats(userId));

        var task = new Task(
            userId,
            "Task",
            "Desc",
            TaskDifficultyType.Easy,
            30,
            FocusUp.Domain.Enums.TaskStatus.Completed
        );

        var taskId = _taskRepository.Insert(task);

        Action action = () =>
            _taskCompletionService.CompleteTask(taskId, userId);

        action.Should().Throw<TaskAlreadyCompletedException>();
    }

    /// <summary>
    /// Tests whether a user cannot complete a task
    /// that belongs to another user.
    /// </summary>
    [Fact]
    public void CompleteTask_WrongUser_ShouldThrowUnauthorizedException()
    {
        var ownerId = TestDatabaseHelper.InsertUser();
        var wrongUserId = TestDatabaseHelper.InsertUser();

        _userStatsRepository.Insert(new UserStats(ownerId));
        _userStatsRepository.Insert(new UserStats(wrongUserId));

        var taskId = _taskRepository.Insert(
            new Task(
                ownerId,
                "Task",
                "Desc",
                TaskDifficultyType.Easy,
                30,
                FocusUp.Domain.Enums.TaskStatus.Open
            )
        );

        Action action = () =>
            _taskCompletionService.CompleteTask(taskId, wrongUserId);

        action.Should().Throw<UnauthorizedTaskAccessException>();
    }

    /// <summary>
    /// Tests whether completing a task creates exactly one XP event.
    /// </summary>
    [Fact]
    public void CompleteTask_ShouldAwardXpExactlyOnce()
    {
        var userId = TestDatabaseHelper.InsertUser();

        _userStatsRepository.Insert(new UserStats(userId));

        var taskId = _taskRepository.Insert(
            new Task(
                userId,
                "Task",
                "Desc",
                TaskDifficultyType.Easy,
                30,
                FocusUp.Domain.Enums.TaskStatus.Open
            )
        );

        _taskCompletionService.CompleteTask(taskId, userId);

        var xpEvents = _xpEventRepository.GetAllByTaskId(taskId);

        xpEvents.Should().HaveCount(1);
    }

    /// <summary>
    /// Tests whether completing a task updates the user's streak.
    /// </summary>
    [Fact]
    public void CompleteTask_ShouldUpdateStreak()
    {
        var userId = TestDatabaseHelper.InsertUser();

        _userStatsRepository.Insert(new UserStats(userId));

        var taskId = _taskRepository.Insert(
            new Task(
                userId,
                "Task",
                "Desc",
                TaskDifficultyType.Easy,
                30,
                FocusUp.Domain.Enums.TaskStatus.Open
            )
        );

        _taskCompletionService.CompleteTask(taskId, userId);

        var stats = _userStatsRepository.GetByUserId(userId);

        stats.Should().NotBeNull();
        stats!.StreakCount.Should().Be(1);
    }

    /// <summary>
    /// Tests whether completing a task triggers badge checks
    /// and awards a matching badge to the user.
    /// </summary>
    [Fact]
    public void CompleteTask_ShouldTriggerBadgeCheck()
    {
        var userId = TestDatabaseHelper.InsertUser();

        var stats = new UserStats(userId);
        stats.AddXp(1000);

        _userStatsRepository.Insert(stats);
        _userStatsRepository.UpdateTotalXp(userId, 1000);

        _badgeRepository.Insert(
            new Badge(
                "XP Badge",
                "Earn XP",
                BadgeRuleType.xp_total,
                100,
                BadgeRarity.Common,
                "#FFFFFF",
                "#000000"
            )
        );

        var taskId = _taskRepository.Insert(
            new Task(
                userId,
                "Task",
                "Desc",
                TaskDifficultyType.Easy,
                30,
                FocusUp.Domain.Enums.TaskStatus.Open
            )
        );

        _taskCompletionService.CompleteTask(taskId, userId);

        var userBadges = _userBadgeRepository.GetByUserId(userId);

        userBadges.Should().NotBeEmpty();
    }

    /// <summary>
    /// Tests whether completing a task creates one task log entry
    /// with the TaskCompleted action.
    /// </summary>
    [Fact]
    public void CompleteTask_ShouldCreateTaskLog()
    {
        var userId = TestDatabaseHelper.InsertUser();

        _userStatsRepository.Insert(new UserStats(userId));

        var taskId = _taskRepository.Insert(
            new Task(
                userId,
                "Task",
                "Desc",
                TaskDifficultyType.Easy,
                30,
                FocusUp.Domain.Enums.TaskStatus.Open
            )
        );

        _taskCompletionService.CompleteTask(taskId, userId);

        var logs = _taskLogRepository.GetAllByTaskId(taskId);

        logs.Should().HaveCount(1);
        logs[0].Action.Should().Be(RewardReason.TaskCompleted);
    }
}