using Microsoft.Data.Sqlite;
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
            var connection = _dbConnection.GetConnection();
            var cmd = connection.CreateCommand();

            cmd.CommandText = $@"SELECT * FROM {_tableName}
                                 WHERE id = @id";
            cmd.Parameters.AddWithValue("@id", id);

            using var reader = cmd.ExecuteReader();

            if (!reader.Read())
                return null;
            return MapToUserBadge(reader);
        }

        public List<UserBadge> GetByUserId(int userId)
        {
            var connection = _dbConnection.GetConnection();
            var cmd = connection.CreateCommand();

            cmd.CommandText = $@"SELECT * FROM {_tableName}
                                 WHERE user_id = @user_id";
            cmd.Parameters.AddWithValue("@user_id", userId);

            using var reader = cmd.ExecuteReader();

            var userBadges = new List<UserBadge>();
            while (reader.Read())
                userBadges.Add(MapToUserBadge(reader));
            return userBadges;
        }

        public List<UserBadge> GetByBadgeId(int badgeId)
        {
            var connection = _dbConnection.GetConnection();
            var cmd = connection.CreateCommand();

            cmd.CommandText = $@"SELECT * FROM {_tableName}
                                 WHERE badge_id = @badge_id";
            cmd.Parameters.AddWithValue("@badge_id", badgeId);

            using var reader = cmd.ExecuteReader();
            var userBadges = new List<UserBadge>();
            while (reader.Read())
                userBadges.Add(MapToUserBadge(reader));
            return userBadges;

        }

        public override int Insert(UserBadge userBadge)
        {
            var connection = _dbConnection.GetConnection();
            var cmd = connection.CreateCommand();

            cmd.CommandText = $@"INSERT INTO {_tableName} (user_id, badge_id, awarded_at)
                                 VALUES (@user_id, @badge_id, @awarded_at);
                                 SELECT last_insert_rowid()";

            cmd.Parameters.AddWithValue("@user_id", userBadge.UserId);
            cmd.Parameters.AddWithValue("@badge_id", userBadge.BadgeId);
            cmd.Parameters.AddWithValue("@awarded_at", userBadge.AwardedAt);

            var id = cmd.ExecuteScalar();

            return Convert.ToInt32(id);
        }

        public bool Exists(int userId, int badgeId)
        {
            var connection = _dbConnection.GetConnection();
            var cmd = connection.CreateCommand();

            cmd.CommandText = $@"SELECT 1 FROM {_tableName}
                                 WHERE user_id = @user_id AND badge_id = @badge_id";

            cmd.Parameters.AddWithValue("@user_id", userId);
            cmd.Parameters.AddWithValue("@badge_id", badgeId);

            using var reader = cmd.ExecuteReader();

            if (!reader.Read())
                return false;
            return true;
        }

        private static UserBadge MapToUserBadge(SqliteDataReader reader)
        {
            UserBadge userBadge = new UserBadge(
                    reader.GetInt32(reader.GetOrdinal("user_id")),
                    reader.GetInt32(reader.GetOrdinal("badge_id")),
                    reader.GetDateTime(reader.GetOrdinal("awarded_at"))
                );

            userBadge.SetId(reader.GetInt32(reader.GetOrdinal("id")));

            return userBadge;
        }
    }
}