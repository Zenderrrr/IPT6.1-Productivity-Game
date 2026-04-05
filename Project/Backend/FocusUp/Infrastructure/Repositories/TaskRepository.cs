using System;

public class TaskRepository : BaseRepository<Task>
{
    public TaskRepository(DatabaseConnection databaseConnection) : base(databaseConnection, "Task")
    {
    }

    public override Task? GetById(int id)
    {
        return base.GetById(id);
    }

    public List<Task> GetAllByUserId(int userId)
    {
        throw new NotImplementedException();
    }

    public List<Task> GetOpenByUserId(int userId)
    {
        throw new NotImplementedException();
    }

    public List<Task> GetCompletedByUserId(int userId)
    {
        throw new NotImplementedException();
    }

    public override int Insert(Task entity)
    {
        return base.Insert(entity);
    }

    public override void Update(Task entity)
    {
        base.Update(entity);
    }

    public override void Delete(int id)
    {
        base.Delete(id);
    }

    public bool Exists(int id)
    {
        throw new NotImplementedException();
    }

    public void UpdateStatus(int taskId, string status, DateTime? completedAt)
    {
        throw new NotImplementedException();
    }

    public int CountOpenTasks(int userId)
    {
        throw new NotImplementedException();
    }

    public int CountCompletedTasks()
    {
        throw new NotImplementedException();
    }
}