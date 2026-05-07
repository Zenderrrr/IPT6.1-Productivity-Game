using FluentAssertions;
using Xunit;
using FocusUp.Domain.Enums;

namespace FocusUp.Tests.Models;

public class TaskModelTests
{
    [Fact]
    public void ValidateData_WithValidTask_ShouldReturnTrue()
    {
        var task = new Task(1, "Task", "Desc", TaskDifficultyType.Easy, 30, FocusUp.Domain.Enums.TaskStatus.Open);

        task.ValidateData().Should().BeTrue();
    }

    [Fact]
    public void ValidateData_WithEmptyTitle_ShouldReturnFalse()
    {
        var task = new Task(1, "", "Desc", TaskDifficultyType.Easy, 30, FocusUp.Domain.Enums.TaskStatus.Open);

        task.ValidateData().Should().BeFalse();
    }

    [Fact]
    public void MarkAsCompleted_ShouldSetStatusToCompleted()
    {
        var task = new Task(1, "Task", "Desc", TaskDifficultyType.Easy, 30, FocusUp.Domain.Enums.TaskStatus.Open);

        task.MarkAsCompleted();

        task.IsCompleted().Should().BeTrue();
    }
}