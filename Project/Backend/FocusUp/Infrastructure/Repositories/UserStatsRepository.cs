using System;

public class UserStatsRepository : BaseRepository<UserStats>
{
    public UserStatsRepository(DatabaseConnection databaseConnection) : base(databaseConnection, "UserStats")
    {
    }

    public override UserStats? GetById(int id)
    {
        return base.GetById(id);
    }

    public UserStats? GetByUserId()
    {
        throw new NotImplementedException();
    }

    public override int Insert(UserStats entity)
    {
        return base.Insert(entity);
    }

    public override void Update(UserStats entity)
    {
        base.Update(entity);
    }

    public bool ExistsByUserId(int userId)
    {
        throw new NotImplementedException();
    }

    public void UpdateTotalXp(int userId, int totalXp)
    {
        throw new NotImplementedException();
    }

    public void UpdateStreak(int userId, int streakCount, int bestStreak, DateTime? streakLastDate)
    {
        throw new NotImplementedException();
    }

    public void UpdateTaskCounters(int userId, int streakCount, int bestStreak, DateTime? streakLastDate)
    {
        throw new NotImplementedException();
    }

    public void UpdateLastActive(int userId, DateTime lastActiveAt)
    {
        throw new NotImplementedException();
    }

    public void DeleteByUserId(int userId)
    {
        throw new NotImplementedException();
    }
}