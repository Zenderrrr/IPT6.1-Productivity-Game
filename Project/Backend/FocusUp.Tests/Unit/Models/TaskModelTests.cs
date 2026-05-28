using FluentAssertions;
using Xunit;
using FocusUp.Domain.Enums;

namespace FocusUp.Tests.Models;

/// <summary>
/// Tests for the Task model.
/// </summary>
public class TaskModelTests
{
    /// <summary>
    /// Ensures valid task data passes validation.
    /// </summary>
    [Fact]
    public void ValidateData_WithValidTask_ShouldReturnTrue()
    {
        var task = new Task(1, "Task", "Desc", TaskDifficultyType.Easy, 30, FocusUp.Domain.Enums.TaskStatus.Open);

        task.ValidateData().Should().BeTrue();
    }

    /// <summary>
    /// Ensures tasks with empty titles fail validation.
    /// </summary>
    [Fact]
    public void ValidateData_WithEmptyTitle_ShouldReturnFalse()
    {
        var task = new Task(1, "", "Desc", TaskDifficultyType.Easy, 30, FocusUp.Domain.Enums.TaskStatus.Open);

        task.ValidateData().Should().BeFalse();
    }

    /// <summary>
    /// Ensures MarkAsCompleted changes task status correctly.
    /// </summary>
    [Fact]
    public void MarkAsCompleted_ShouldSetStatusToCompleted()
    {
        var task = new Task(1, "Task", "Desc", TaskDifficultyType.Easy, 30, FocusUp.Domain.Enums.TaskStatus.Open);

        task.MarkAsCompleted();

        task.IsCompleted().Should().BeTrue();
    }
}