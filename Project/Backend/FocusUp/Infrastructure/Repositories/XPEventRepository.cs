using FocusUp.Domain.Enums;
using Microsoft.Data.Sqlite;
using System;

namespace FocusUp.Infrastructure.Repositories
{
    public class XPEventRepository : BaseRepository<XpEvent>
    {
        public XPEventRepository(DatabaseConnection databaseConnection) : base(databaseConnection, "XpEvent")
        {
        }

        public override XpEvent? GetById(int id)
        {
            var connection = _dbConnection.GetConnection();
            using var cmd = connection.CreateCommand();

            cmd.CommandText = $@"SELECT * FROM {_tableName}
                                 WHERE id = @id";
            cmd.Parameters.AddWithValue("@id", id);

            using var reader = cmd.ExecuteReader();

            if (!reader.Read())
                return null;
            return MapToXpEvent(reader);
        }


        public List<XpEvent> GetAllByUserId(int userId)
        {
            var connection = _dbConnection.GetConnection();
            using var cmd = connection.CreateCommand();

            cmd.CommandText = $@"SELECT * FROM {_tableName}
                                 WHERE user_id = @user_id";
            cmd.Parameters.AddWithValue("@user_id", userId);

            using var reader = cmd.ExecuteReader();

            var xpEvents = new List<XpEvent>();
            while (reader.Read())
                xpEvents.Add(MapToXpEvent(reader));
            return xpEvents;
        }

        public List<XpEvent> GetAllByTaskId(int taskId)
        {
            var connection = _dbConnection.GetConnection();
            using var cmd = connection.CreateCommand();

            cmd.CommandText = $@"SELECT * FROM {_tableName}
                                 WHERE task_id = @task_id";
            cmd.Parameters.AddWithValue("@task_id", taskId);

            using var reader = cmd.ExecuteReader();

            var xpEvents = new List<XpEvent>();
            while (reader.Read())
                xpEvents.Add(MapToXpEvent(reader));
            return xpEvents;
        }

        public override int Insert(XpEvent xpEvent)
        {
            var connection = _dbConnection.GetConnection();
            using var cmd = connection.CreateCommand();

            cmd.CommandText = $@"INSERT INTO {_tableName} (user_id, task_id, reason, amount)
                                 VALUES (@user_id, @task_id, @reason, @amount);
                                SELECT last_insert_rowid()";

            cmd.Parameters.AddWithValue("@user_id", xpEvent.UserId);
            cmd.Parameters.AddWithValue("@task_id", xpEvent.TaskId ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@reason", xpEvent.Reason);
            cmd.Parameters.AddWithValue("@amount", xpEvent.Amount);

            var id = cmd.ExecuteScalar();

            return Convert.ToInt32(id);
        }

        public int Insert(XpEvent xpEvent, SqliteConnection connection, SqliteTransaction transaction)
        {
            using var cmd = connection.CreateCommand();
            cmd.Transaction = transaction;

            cmd.CommandText = $@"INSERT INTO {_tableName} (user_id, task_id, reason, amount)
                                 VALUES (@user_id, @task_id, @reason, @amount);
                                SELECT last_insert_rowid()";

            cmd.Parameters.AddWithValue("@user_id", xpEvent.UserId);
            cmd.Parameters.AddWithValue("@task_id", xpEvent.TaskId ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@reason", xpEvent.Reason);
            cmd.Parameters.AddWithValue("@amount", xpEvent.Amount);

            var id = cmd.ExecuteScalar();

            return Convert.ToInt32(id);
        }

        public int GetTotalXpByUserId(int userId)
        {
            var connection = _dbConnection.GetConnection();
            using var cmd = connection.CreateCommand();

            cmd.CommandText = $@"SELECT SUM(amount) AS 'TotalXP' FROM {_tableName}
                                 WHERE user_id = @user_id";

            cmd.Parameters.AddWithValue("@user_id", userId);
            var amount = cmd.ExecuteScalar();

            return Convert.ToInt32(amount);
        }

        public int GetTotalXpByUserId(int userId, DateTime from, DateTime to)
        {
            var connection = _dbConnection.GetConnection();
            using var cmd = connection.CreateCommand();

            cmd.CommandText = $@"SELECT SUM(amount) AS 'TotalXP' FROM {_tableName}
                                 WHERE user_id = @user_id AND (created_at BETWEEN @from AND @to)";
            cmd.Parameters.AddWithValue("@user_id", userId);
            cmd.Parameters.AddWithValue("@from", from);
            cmd.Parameters.AddWithValue("@to", to);

            var amount = cmd.ExecuteScalar();

            if (amount == null || amount == DBNull.Value)
                return 0;

            return Convert.ToInt32(amount);
        }

        public List<(DateTime date, int xp)> GetXpPerDay(int userId, DateTime from, DateTime to)
        {
            var connection = _dbConnection.GetConnection();
            using var cmd = connection.CreateCommand();

            cmd.CommandText = $@"SELECT DATE(created_at) AS date, SUM(amount) AS xp FROM {_tableName}
                                 WHERE user_id = @user_id AND (created_at BETWEEN @from AND @to)
                                 GROUP BY DATE(created_at)
                                 ORDER BY date";
            cmd.Parameters.AddWithValue("@user_id", userId);
            cmd.Parameters.AddWithValue("@from", from);
            cmd.Parameters.AddWithValue("@to", to);

            using var reader = cmd.ExecuteReader();

            Dictionary<DateTime, int> xpPerDayList = new();
            while (reader.Read())
                xpPerDayList[DateTime.Parse(reader.GetString(0))] = Convert.ToInt32(reader.GetValue(1));

            List<DateTime> days = new();
            for (int i = 0; i <= (to.Date - from.Date).Days; i++)
                days.Add(from.Date.AddDays(i));

            List<(DateTime date, int xp)> xpPerDay = new();
            foreach (var day in days)
            {
                if (!xpPerDayList.TryGetValue(day, out int xp))
                    xp = 0;

                xpPerDay.Add((day, xp));
            }
            return xpPerDay;
        }

        public bool ExistsForTask(int taskId, RewardReason reason)
        {
            var connection = _dbConnection.GetConnection();
            using var cmd = connection.CreateCommand();

            cmd.CommandText = $@"SELECT 1 FROM {_tableName}
                                 WHERE task_id = @task_id AND reason = @reason";

            cmd.Parameters.AddWithValue("@task_id", taskId);
            cmd.Parameters.AddWithValue("@reason", reason.ToString());

            using var reader = cmd.ExecuteReader();

            if (!reader.Read())
                return false;
            return true;
        }

        public List<XpEvent> GetRecentByUserId(int userId, int limit)
        {
            var connection = _dbConnection.GetConnection();
            using var cmd = connection.CreateCommand();

            cmd.CommandText = $@"SELECT * FROM {_tableName}
                                 WHERE user_id = @user_id
                                 LIMIT {limit}";

            cmd.Parameters.AddWithValue("@user_id", userId);

            using var reader = cmd.ExecuteReader();

            var xpEvents = new List<XpEvent>();
            while (reader.Read())
                xpEvents.Add(MapToXpEvent(reader));
            return xpEvents;
        }

        private static XpEvent MapToXpEvent(SqliteDataReader reader)
        {
            int taskIdx = reader.GetOrdinal("task_id");
            int? taskId = reader.IsDBNull(taskIdx) ? null : reader.GetInt32(taskIdx);

            XpEvent xpEvent = new XpEvent(
                    reader.GetInt32(reader.GetOrdinal("user_id")),
                    reader.GetInt32(reader.GetOrdinal("amount")),
                    Enum.Parse<RewardReason>(reader.GetString(reader.GetOrdinal("reason"))),
                    taskId
                );

            xpEvent.SetId(reader.GetInt32(reader.GetOrdinal("id")));
            xpEvent.SetCreatedAt(reader.GetDateTime(reader.GetOrdinal("created_at")));

            return xpEvent;
        }
    }
}