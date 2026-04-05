using System;

public class BadgeRepository : BaseRepository<Badge>
{
    public BadgeRepository(DatabaseConnection databaseConnection) : base(databaseConnection, "Badge")
    {
    }

    public override Badge? GetById(int id)
    {
        return base.GetById(id);
    }

    public List<Badge> GetAll()
    {
        throw new NotImplementedException();
    }

    public List<Badge> GetByRuleType(string ruleType)
    {
        throw new NotImplementedException();
    }

    public override int Insert(Badge entity)
    {
        return base.Insert(entity);
    }

    public override void Update(Badge entity)
    {
        base.Update(entity);
    }

    public override void Delete(int id)
    {
        base.Delete(id);
    }

    public bool ExistsByName(string name)
    {
        throw new NotImplementedException();
    }
}