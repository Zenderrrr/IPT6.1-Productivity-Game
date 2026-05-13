using FluentAssertions;
using Xunit;
using FocusUp.Application.Services;
using FocusUp.Application.Strategies;
using FocusUp.Domain.Enums;
using FocusUp.Infrastructure.Repositories;
using FocusUp.Common.Exceptions;

namespace FocusUp.Tests.Services;

public class TaskCompletionServiceTests : IDisposable
{
    private readonly DatabaseConnection _db;

    private readonly TaskRepository _taskRepository;
    private readonly UserStatsRepository _userStatsRepository;
    private readonly XPEventRepository _xpEventRepository;
    private readonly TaskLogRepository _taskLogRepository;
    private readonly BadgeRepository _badgeRepository;
    private readonly UserBadgeRepository _userBadgeRepository;

    private readonly TaskCompletionService _taskCompletionService;

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

    public void Dispose()
    {
        TestDatabaseHelper.Clean();
    }

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
                100
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