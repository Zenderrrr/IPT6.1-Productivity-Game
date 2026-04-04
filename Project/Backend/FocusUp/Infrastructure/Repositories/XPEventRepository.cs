using System;

public class XPEventRepository : BaseRepository<XpEvent>
{
    public XPEventRepository(DatabaseConnection databaseConnection) : base(databaseConnection, "XpEvent")
    {
    }

    public override XpEvent? GetById(int id)
    {
        return base.GetById(id);
    }

    public List<XpEvent> GetAllByUserId(int userId)
    {
        throw new NotImplementedException();
    }

    public List<XpEvent> GetAllByTaskId(int taskId)
    {
        throw new NotImplementedException();
    }

    public override int Insert(XpEvent entity)
    {
        return base.Insert(entity);
    }

    public override void Delete(int id)
    {
        base.Delete(id);
    }

    public int GetTotalXpByUserId(int userId)
    {
        throw new NotImplementedException();
    }

    public bool ExistsForTask(int taskId, string reason)
    {
        throw new NotImplementedException();
    }

    public List<XpEvent> GetRecentByUserId(int userId, int limit)
    {
        throw new NotImplementedException();
    }
}