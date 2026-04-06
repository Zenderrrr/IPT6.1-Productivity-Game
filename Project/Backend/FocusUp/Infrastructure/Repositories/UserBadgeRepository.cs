using System;

namespace FocusUp.Infrastructure.Repositories
{
    public class UserBadgeRepository : BaseRepository<UserBadge>
    {
        public UserBadgeRepository(DatabaseConnection databaseConnection) : base(databaseConnection, "UserBadge")
        {
        }

        public override UserBadge? GetById(int id)
        {
            return base.GetById(id);
        }

        public List<UserBadge> GetByUserId(int userId)
        {
            throw new NotImplementedException();
        }

        public List<UserBadge> GetByBadgeId(int badgeId)
        {
            throw new NotImplementedException();
        }

        public override int Insert(UserBadge entity)
        {
            return base.Insert(entity);
        }

        public override void Delete(int id)
        {
            base.Delete(id);
        }

        public bool Exists(int userId, int badgeId)
        {
            throw new NotImplementedException();
        }
    }
}