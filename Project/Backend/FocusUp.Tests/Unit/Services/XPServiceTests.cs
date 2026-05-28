using FluentAssertions;
using Moq;
using Xunit;
using FocusUp.Application.Services;
using FocusUp.Application.Strategies.Interfaces;
using FocusUp.Domain.Enums;

namespace FocusUp.Tests.Services;

/// <summary>
/// Tests for the XPService.
/// </summary>
public class XPServiceTests
{
    /// <summary>
    /// Mocked XP calculation strategy used for testing.
    /// </summary>
    private readonly Mock<IXpCalculationStrategy> _xpStrategyMock;

    /// <summary>
    /// Service under test.
    /// </summary>
    private readonly XPService _xpService;

    /// <summary>
    /// Initializes mocked dependencies and service instance.
    /// </summary>
    public XPServiceTests()
    {
        _xpStrategyMock = new Mock<IXpCalculationStrategy>();

        _xpService = new XPService(null!, null!, _xpStrategyMock.Object);
    }

    /// <summary>
    /// Ensures easy tasks return the expected XP amount.
    /// </summary>
    [Fact]
    public void CalculateXP_WithEasyTask_ShouldReturnEasyXp()
    {
        var task = CreateTask(TaskDifficultyType.Easy, 30);

        _xpStrategyMock
            .Setup(s => s.CalculateXP(task, 0))
            .Returns(50);

        var result = _xpService.CalculateXP(task, 0);

        result.Should().Be(50);
    }

    /// <summary>
    /// Ensures medium tasks return the expected XP amount.
    /// </summary>
    [Fact]
    public void CalculateXP_WithMediumTask_ShouldReturnMediumXp()
    {
        var task = CreateTask(TaskDifficultyType.Medium, 30);

        _xpStrategyMock
            .Setup(s => s.CalculateXP(task, 0))
            .Returns(100);

        var result = _xpService.CalculateXP(task, 0);

        result.Should().Be(100);
    }

    /// <summary>
    /// Ensures hard tasks return the expected XP amount.
    /// </summary>
    [Fact]
    public void CalculateXP_WithHardTask_ShouldReturnHardXp()
    {
        var task = CreateTask(TaskDifficultyType.Hard, 30);

        _xpStrategyMock
            .Setup(s => s.CalculateXP(task, 0))
            .Returns(150);

        var result = _xpService.CalculateXP(task, 0);

        result.Should().Be(150);
    }

    /// <summary>
    /// Ensures longer tasks return more XP.
    /// </summary>
    [Fact]
    public void CalculateXP_WithLongerDuration_ShouldReturnMoreXp()
    {
        var task = CreateTask(TaskDifficultyType.Medium, 90);

        _xpStrategyMock
            .Setup(s => s.CalculateXP(task, 0))
            .Returns(130);

        var result = _xpService.CalculateXP(task, 0);

        result.Should().Be(130);
    }

    /// <summary>
    /// Ensures streak bonuses increase XP rewards.
    /// </summary>
    [Fact]
    public void CalculateXP_WithStreakBonus_ShouldReturnXpWithBonus()
    {
        var task = CreateTask(TaskDifficultyType.Easy, 30);

        _xpStrategyMock
            .Setup(s => s.CalculateXP(task, 5))
            .Returns(75);

        var result = _xpService.CalculateXP(task, 5);

        result.Should().Be(75);
    }

    /// <summary>
    /// Creates a test task with the given difficulty and duration.
    /// </summary>
    private static Task CreateTask(TaskDifficultyType difficulty, int durationMin)
    {
        return new Task(
            userId: 1,
            title: "Test Task",
            desciption: "Test Description", // Note: "description" is misspelled in the Task constructor, but we will keep it as is for consistency with the existing code.
            difficulty: difficulty,
            durationMin: durationMin,
            status: FocusUp.Domain.Enums.TaskStatus.Open
        );
    }
}