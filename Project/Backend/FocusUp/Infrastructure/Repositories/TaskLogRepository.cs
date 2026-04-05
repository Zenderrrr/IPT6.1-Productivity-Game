using System;

public class TaskLogRepository : BaseRepository<TaskLog>
{
    public TaskLogRepository(DatabaseConnection databaseConnection) : base(databaseConnection, "TaskLog")
    {
    }

    public override TaskLog? GetById(int id)
    {
        return base.GetById(id);
    }

    public List<TaskLog> GetAllByUserId(int userId)
    {
        throw new NotImplementedException();
    }

    public List<TaskLog> GetAllByTaskId(int taskId)
    {
        throw new NotImplementedException();
    }

    public override int Insert(TaskLog entity)
    {
        return base.Insert(entity);
    }

    public override void Delete(int id)
    {
        base.Delete(id);
    }

    public List<TaskLog> GetRecentByUserId(int userId, int limit)
    {
        throw new NotImplementedException();
    }
}