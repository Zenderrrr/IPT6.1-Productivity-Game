using FluentAssertions;
using Xunit;
using FocusUp.Domain.Enums;
using FocusUp.Infrastructure.Repositories;

namespace FocusUp.Tests.Repositories;

public class RepositoryTests
{
    [Fact]
    public void TaskRepository_Insert_ShouldSaveTask()
    {
        var db = TestDatabaseHelper.Db;
        var userId = TestDatabaseHelper.InsertUser();

        var repo = new TaskRepository(db);

        var task = new Task(userId, "Task", "Desc", TaskDifficultyType.Easy, 30, FocusUp.Domain.Enums.TaskStatus.Open);

        var id = repo.Insert(task);

        id.Should().BeGreaterThan(0);
    }

    [Fact]
    public void TaskRepository_GetById_ShouldReturnCorrectTask()
    {
        var db = TestDatabaseHelper.Db;
        var userId = TestDatabaseHelper.InsertUser();

        var repo = new TaskRepository(db);

        var id = repo.Insert(new Task(userId, "Task", "Desc", TaskDifficultyType.Easy, 30, FocusUp.Domain.Enums.TaskStatus.Open));

        var result = repo.GetById(id);

        result.Should().NotBeNull();
        result!.Title.Should().Be("Task");
        result.UserId.Should().Be(userId);
    }

    [Fact]
    public void TaskRepository_Update_ShouldSaveChanges()
    {
        var db = TestDatabaseHelper.Db;
        var userId = TestDatabaseHelper.InsertUser();

        var repo = new TaskRepository(db);

        var id = repo.Insert(new Task(userId, "Old", "Desc", TaskDifficultyType.Easy, 30, FocusUp.Domain.Enums.TaskStatus.Open));

        var updated = new Task(id, userId, "New", "Updated", TaskDifficultyType.Hard, 60, FocusUp.Domain.Enums.TaskStatus.Open);

        repo.Update(updated);

        var result = repo.GetById(id);

        result!.Title.Should().Be("New");
        result.Description.Should().Be("Updated");
        result.Difficulty.Should().Be(TaskDifficultyType.Hard);
        result.DurationMin.Should().Be(60);
    }

    [Fact]
    public void TaskRepository_Delete_ShouldDeleteTask()
    {
        var db = TestDatabaseHelper.Db;
        var userId = TestDatabaseHelper.InsertUser();

        var repo = new TaskRepository(db);

        var id = repo.Insert(new Task(userId, "Task", "Desc", TaskDifficultyType.Easy, 30, FocusUp.Domain.Enums.TaskStatus.Open));

        repo.Delete(id);

        repo.GetById(id).Should().BeNull();
    }

    [Fact]
    public void DeletingUser_ShouldCascadeDeleteTasks()
    {
        var db = TestDatabaseHelper.Db;
        var userId = TestDatabaseHelper.InsertUser();

        var repo = new TaskRepository(db);

        var taskId = repo.Insert(new Task(userId, "Task", "Desc", TaskDifficultyType.Easy, 30, FocusUp.Domain.Enums.TaskStatus.Open));

        var connection = db.GetConnection();
        using var cmd = connection.CreateCommand();
        cmd.CommandText = "DELETE FROM User WHERE id = @id";
        cmd.Parameters.AddWithValue("@id", userId);
        cmd.ExecuteNonQuery();

        repo.GetById(taskId).Should().BeNull();
    }
}