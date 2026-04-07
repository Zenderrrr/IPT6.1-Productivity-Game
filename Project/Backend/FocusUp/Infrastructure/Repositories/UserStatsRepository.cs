using Microsoft.Data.Sqlite;
using System;

namespace FocusUp.Infrastructure.Repositories
{
    public class UserStatsRepository : BaseRepository<UserStats>
    {
        public UserStatsRepository(DatabaseConnection databaseConnection) : base(databaseConnection, "UserStats")
        {
        }

        public override UserStats? GetById(int id)
        {
            var connection = _dbConnection.GetConnection();
            var cmd = connection.CreateCommand();

            cmd.CommandText = $@"SELECT * FROM {_tableName}
                                 WHERE id = @id";
            cmd.Parameters.AddWithValue("@id", id);

            using var reader = cmd.ExecuteReader();

            if (!reader.Read())
                return null;
            return MapToUserStats(reader);
        }

        public UserStats? GetByUserId(int userId)
        {
            var connection = _dbConnection.GetConnection();
            var cmd = connection.CreateCommand();

            cmd.CommandText = $@"SELECT * FROM {_tableName}
                                 WHERE user_id = @user_id";
            cmd.Parameters.AddWithValue("@user_id", userId);

            using var reader = cmd.ExecuteReader();

            if (!reader.Read())
                return null;
            return MapToUserStats(reader);
        }

        public override int Insert(UserStats userStats)
        {
            var connection = _dbConnection.GetConnection();
            using var cmd = connection.CreateCommand();

            cmd.CommandText = $@"INSERT INTO {_tableName} (user_id)
                                 VALUES (@user_id);
                                 SELECT last_insert_rowid()";

            cmd.Parameters.AddWithValue("@user_id", userStats.UserId);

            var id = cmd.ExecuteScalar();
            userStats.SetId(Convert.ToInt32(id));
            return Convert.ToInt32(id);
        }

        public int Insert(UserStats userStats, SqliteConnection connection, SqliteTransaction transaction)
        {
            using var cmd = connection.CreateCommand();

            cmd.Transaction = transaction;

            cmd.CommandText = $@"INSERT INTO {_tableName} (user_id)
                                 VALUES (@user_id);
                                 SELECT last_insert_rowid()";

            cmd.Parameters.AddWithValue("@user_id", userStats.UserId);

            var id = cmd.ExecuteScalar();
            userStats.SetId(Convert.ToInt32(id));
            return Convert.ToInt32(id);
        }

        public override void Update(UserStats userStats)
        {
            var connection = _dbConnection.GetConnection();
            var cmd = connection.CreateCommand();

            cmd.CommandText = $@"UPDATE {_tableName}
                                 SET total_xp = @total_xp, tasks_done = @tasks_done, tasks_open = @tasks_open, total_time_min = @total_time_min, streak_count = @streak_count, best_streak = @best_streak, streak_last_date = @streak_last_date, last_active_at = @last_active_at, updated_at = @updated_at
                                 WHERE id = @id";

            cmd.Parameters.AddWithValue("@id", userStats.Id);
            
            cmd.Parameters.AddWithValue("@total_xp", userStats.TotalXp);
            cmd.Parameters.AddWithValue("@tasks_done", userStats.TasksDone);
            cmd.Parameters.AddWithValue("@tasks_open", userStats.TasksOpen);
            cmd.Parameters.AddWithValue("@total_time_min", userStats.TotalTimeMin);
            cmd.Parameters.AddWithValue("@streak_count", userStats.StreakCount);
            cmd.Parameters.AddWithValue("@best_streak", userStats.BestStreak);
            cmd.Parameters.AddWithValue("@streak_last_date", userStats.StreakLastDate ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@last_active_at", userStats.LastActiveAt ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@updated_at", userStats.UpdatedAt);

            cmd.ExecuteNonQuery();
        }

        public bool ExistsByUserId(int userId)
        {
            var connection = _dbConnection.GetConnection();
            var cmd = connection.CreateCommand();

            cmd.CommandText = $@"SELECT 1 FROM {_tableName}
                                 WHERE user_id = @user_id";

            cmd.Parameters.AddWithValue("@user_id", userId);

            using var reader = cmd.ExecuteReader();

            if (!reader.Read())
                return false;
            return true;
        }

        public void UpdateTotalXp(int userId, int totalXp)
        {
            var connection = _dbConnection.GetConnection();
            var cmd = connection.CreateCommand();

            cmd.CommandText = $@"UPDATE {_tableName}
                                 SET total_xp = @total_xp, updated_at = @updated_at
                                 WHERE user_id = @user_id";

            cmd.Parameters.AddWithValue("@user_id", userId);
            cmd.Parameters.AddWithValue("@total_xp", totalXp);
            cmd.Parameters.AddWithValue("@updated_at", DateTime.Now);

            cmd.ExecuteNonQuery();
        }

        public void UpdateStreak(int userId, int streakCount, int bestStreak, DateTime? streakLastDate)
        {
            var connection = _dbConnection.GetConnection();
            var cmd = connection.CreateCommand();

            cmd.CommandText = $@"UPDATE {_tableName}
                                 SET streak_count = @streak_count, best_streak = @best_streak, streak_last_date = @streak_last_date, updated_at = @updated_at
                                 WHERE user_id = @user_id";

            cmd.Parameters.AddWithValue("@user_id", userId);
            cmd.Parameters.AddWithValue("@streak_count", streakCount);
            cmd.Parameters.AddWithValue("@best_streak", bestStreak);
            cmd.Parameters.AddWithValue("@streak_last_date", streakLastDate ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@updated_at", DateTime.Now);

            cmd.ExecuteNonQuery();
        }

        public void UpdateTaskCounters(int userId, int tasksDone, int tasksOpen)
        {
            var connection = _dbConnection.GetConnection();
            var cmd = connection.CreateCommand();

            cmd.CommandText = $@"UPDATE {_tableName}
                                 SET tasks_done = @tasks_done, tasks_open = @tasks_open, updated_at = @updated_at
                                 WHERE user_id = @user_id";

            cmd.Parameters.AddWithValue("@user_id", userId);
            cmd.Parameters.AddWithValue("@tasks_done", tasksDone);
            cmd.Parameters.AddWithValue("@tasks_open", tasksOpen);
            cmd.Parameters.AddWithValue("@updated_at", DateTime.Now);

            cmd.ExecuteNonQuery();
        }

        public void UpdateLastActive(int userId, DateTime lastActiveAt)
        {
            var connection = _dbConnection.GetConnection();
            var cmd = connection.CreateCommand();

            cmd.CommandText = $@"UPDATE {_tableName}
                                 SET last_active_at = @last_active_at, updated_at = @updated_at
                                 WHERE user_id = @user_id";

            cmd.Parameters.AddWithValue("@user_id", userId);
            cmd.Parameters.AddWithValue("@last_active_at", lastActiveAt);
            cmd.Parameters.AddWithValue("@updated_at", DateTime.Now);

            cmd.ExecuteNonQuery();
        }

        public void DeleteByUserId(int userId)
        {
            var connection = _dbConnection.GetConnection();
            var cmd = connection.CreateCommand();

            cmd.CommandText = $@"DELETE FROM {_tableName}
                                 WHERE user_id = @user_id";

            cmd.Parameters.AddWithValue("@user_id", userId);

            cmd.ExecuteNonQuery();
        }

        private static UserStats MapToUserStats(SqliteDataReader reader)
        {
            int streakLastDateId = reader.GetOrdinal("streak_last_date");
            int lastActiveAtId = reader.GetOrdinal("last_active_at");

            DateTime? streakLastDate = reader.IsDBNull(streakLastDateId) ? null : reader.GetDateTime(streakLastDateId);
            DateTime? lastActiveAt = reader.IsDBNull(lastActiveAtId) ? null : reader.GetDateTime(lastActiveAtId);

            UserStats userStats = new UserStats(
                    reader.GetInt32(reader.GetOrdinal("user_id")),
                    reader.GetInt32(reader.GetOrdinal("total_xp")),
                    reader.GetInt32(reader.GetOrdinal("tasks_done")),
                    reader.GetInt32(reader.GetOrdinal("tasks_open")),
                    reader.GetInt32(reader.GetOrdinal("total_time_min")),
                    reader.GetInt32(reader.GetOrdinal("streak_count")),
                    reader.GetInt32(reader.GetOrdinal("best_streak")),
                    streakLastDate,
                    lastActiveAt
                );

            userStats.SetId(reader.GetInt32(reader.GetOrdinal("id")));
            userStats.SetCreatedAt(reader.GetDateTime(reader.GetOrdinal("created_at")));
            userStats.SetUpdatedAt(reader.GetDateTime(reader.GetOrdinal("updated_at")));

            return userStats;
        }
    }
}