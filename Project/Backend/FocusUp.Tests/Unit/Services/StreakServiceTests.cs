using FluentAssertions;
using Moq;
using Xunit;
using FocusUp.Application.Services;
using FocusUp.Application.Strategies.Interfaces;

namespace FocusUp.Tests.Services;

/// <summary>
/// Tests for the StreakService.
/// </summary>
public class StreakServiceTests
{
    /// <summary>
    /// Mocked streak rule strategy used for testing.
    /// </summary>
    private readonly Mock<IStreakRuleStrategy> _streakStrategyMock;

    /// <summary>
    /// Service under test.
    /// </summary>
    private readonly StreakService _streakService;

    /// <summary>
    /// Initializes mocked dependencies and service instance.
    /// </summary>
    public StreakServiceTests()
    {
        _streakStrategyMock = new Mock<IStreakRuleStrategy>();
        _streakService = new StreakService(null!, _streakStrategyMock.Object);
    }

    /// <summary>
    /// Ensures the first task completion starts the streak at 1.
    /// </summary>
    [Fact]
    public void CalculateNewStreak_FirstCompletion_ShouldReturn1()
    {
        var completedAt = new DateTime(2026, 5, 3);

        var userStats = new UserStats(
            userId: 1,
            totalXp: 0,
            tasksDone: 0,
            tasksOpen: 0,
            totalTimeMin: 0,
            streakCount: 0,
            bestStreak: 0,
            streakLastDate: null
        );

        _streakStrategyMock
            .Setup(s => s.ShouldResetStreak(userStats, completedAt))
            .Returns(false);

        var result = _streakService.CalculateNewStreak(userStats, completedAt);

        result.Should().Be(1);
    }

    /// <summary>
    /// Ensures completing another task on the same day
    /// does not increase the streak.
    /// </summary>
    [Fact]
    public void CalculateNewStreak_SameDayCompletion_ShouldNotIncreaseStreak()
    {
        var completedAt = new DateTime(2026, 5, 3);

        var userStats = new UserStats(
            userId: 1,
            totalXp: 0,
            tasksDone: 0,
            tasksOpen: 0,
            totalTimeMin: 0,
            streakCount: 3,
            bestStreak: 3,
            streakLastDate: completedAt
        );

        _streakStrategyMock
            .Setup(s => s.ShouldResetStreak(userStats, completedAt))
            .Returns(false);

        var result = _streakService.CalculateNewStreak(userStats, completedAt);

        result.Should().Be(3);
    }

    /// <summary>
    /// Ensures completing a task on the next day
    /// increases the streak by one.
    /// </summary>
    [Fact]
    public void CalculateNewStreak_NextDayCompletion_ShouldIncreaseBy1()
    {
        var completedAt = new DateTime(2026, 5, 3);

        var userStats = new UserStats(
            userId: 1,
            totalXp: 0,
            tasksDone: 0,
            tasksOpen: 0,
            totalTimeMin: 0,
            streakCount: 3,
            bestStreak: 3,
            streakLastDate: completedAt.AddDays(-1)
        );

        _streakStrategyMock
            .Setup(s => s.ShouldResetStreak(userStats, completedAt))
            .Returns(false);

        var result = _streakService.CalculateNewStreak(userStats, completedAt);

        result.Should().Be(4);
    }

    /// <summary>
    /// Ensures a gap of more than one day
    /// resets the streak to 1.
    /// </summary>
    [Fact]
    public void CalculateNewStreak_GapMoreThanOneDay_ShouldResetTo1()
    {
        var completedAt = new DateTime(2026, 5, 3);

        var userStats = new UserStats(
            userId: 1,
            totalXp: 0,
            tasksDone: 0,
            tasksOpen: 0,
            totalTimeMin: 0,
            streakCount: 5,
            bestStreak: 5,
            streakLastDate: completedAt.AddDays(-3)
        );

        _streakStrategyMock
            .Setup(s => s.ShouldResetStreak(userStats, completedAt))
            .Returns(true);

        var result = _streakService.CalculateNewStreak(userStats, completedAt);

        result.Should().Be(1);
    }

    /// <summary>
    /// Ensures the best streak is updated when
    /// the current streak becomes higher.
    /// </summary>
    [Fact]
    public void SetStreak_WhenCurrentStreakIsHigherThanBestStreak_ShouldUpdateBestStreak()
    {
        var completedAt = new DateTime(2026, 5, 3);

        var userStats = new UserStats(
            userId: 1,
            totalXp: 0,
            tasksDone: 0,
            tasksOpen: 0,
            totalTimeMin: 0,
            streakCount: 2,
            bestStreak: 2,
            streakLastDate: completedAt.AddDays(-1)
        );

        userStats.SetStreak(3, completedAt);

        userStats.BestStreak.Should().Be(3);
    }
}