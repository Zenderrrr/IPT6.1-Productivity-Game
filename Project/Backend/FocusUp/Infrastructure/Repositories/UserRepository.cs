using System;

public class UserRepository : BaseRepository<User>
{
    public UserRepository(DatabaseConnection databaseConnection) : base(databaseConnection, "User")
    {
    }

    public override User? GetById(int id)
    {
        return base.GetById(id);
    }

    public User? GetByUsername(string username)
    {
        throw new NotImplementedException();
    }

    public User? GetByEmail(string email)
    {
        throw new NotImplementedException();
    }

    public override int Insert(User entity)
    {
        return base.Insert(entity);
    }

    public override void Update(User entity)
    {
        base.Update(entity);
    }

    public override void Delete(int id)
    {
        base.Delete(id);
    }

    public bool ExistsByUsername(string username)
    {
        throw new NotImplementedException();
    }

    public bool ExistsByEmail(string email)
    {
        throw new NotImplementedException();
    }

    public void UpdatePassword(int userId, string passwordHash)
    {
        throw new NotImplementedException();
    }
}