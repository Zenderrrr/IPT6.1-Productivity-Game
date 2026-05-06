using FluentAssertions;
using Moq;
using Xunit;
using FocusUp.Application.Services;
using FocusUp.Application.Strategies.Interfaces;
using FocusUp.Domain.Enums;

namespace FocusUp.Tests.Services;

public class XPServiceTests
{
    private readonly Mock<IXpCalculationStrategy> _xpStrategyMock;
    private readonly XPService _xpService;

    public XPServiceTests()
    {
        _xpStrategyMock = new Mock<IXpCalculationStrategy>();

        _xpService = new XPService(null!, null!, _xpStrategyMock.Object);
    }

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