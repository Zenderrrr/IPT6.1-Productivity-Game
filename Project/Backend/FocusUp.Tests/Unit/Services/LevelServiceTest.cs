using FluentAssertions;
using Moq;
using Xunit;
using FocusUp.Application.Services;
using FocusUp.Application.Strategies.Interfaces;

namespace FocusUp.Tests.Services;

public class LevelServiceTests
{
    private readonly Mock<ILevelStrategy> _levelStrategyMock;
    private readonly LevelService _levelService;

    public LevelServiceTests()
    {
        _levelStrategyMock = new Mock<ILevelStrategy>();
        _levelService = new LevelService(_levelStrategyMock.Object);
    }

    [Fact]
    public void GetLevel_With0Xp_ShouldReturnLevel1()
    {
        _levelStrategyMock.Setup(s => s.CalculateLevel(0)).Returns(1);

        var result = _levelService.GetLevel(0);

        result.Should().Be(1);
    }

    [Fact]
    public void GetLevel_With100Xp_ShouldReturnLevel2()
    {
        _levelStrategyMock.Setup(s => s.CalculateLevel(100)).Returns(2);

        var result = _levelService.GetLevel(100);

        result.Should().Be(2);
    }

    [Fact]
    public void GetLevel_With400Xp_ShouldReturnLevel3()
    {
        _levelStrategyMock.Setup(s => s.CalculateLevel(400)).Returns(3);

        var result = _levelService.GetLevel(400);

        result.Should().Be(3);
    }

    [Fact]
    public void GetProgressToNextLevel_ShouldReturnCorrectProgress()
    {
        _levelStrategyMock
            .Setup(s => s.CalculateProgressToNextLevel(150))
            .Returns(0.5);

        var result = _levelService.GetProgressToNextLevel(150);

        result.Should().Be(0.5);
    }

    [Fact]
    public void GetLevel_DirectlyBeforeLevelUp_ShouldReturnPreviousLevel()
    {
        _levelStrategyMock.Setup(s => s.CalculateLevel(99)).Returns(1);

        var result = _levelService.GetLevel(99);

        result.Should().Be(1);
    }

    [Fact]
    public void GetLevel_DirectlyAfterLevelUp_ShouldReturnNextLevel()
    {
        _levelStrategyMock.Setup(s => s.CalculateLevel(100)).Returns(2);

        var result = _levelService.GetLevel(100);

        result.Should().Be(2);
    }
}