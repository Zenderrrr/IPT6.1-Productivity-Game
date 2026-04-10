using FocusUp.Domain.Enums;
using Microsoft.Data.Sqlite;
using System;

namespace FocusUp.Infrastructure.Repositories
{
    public class TaskLogRepository : BaseRepository<TaskLog>
    {
        public TaskLogRepository(DatabaseConnection databaseConnection) : base(databaseConnection, "TaskLog")
        {
        }

        public override TaskLog? GetById(int id)
        {
            var connection = _dbConnection.GetConnection();

            using var cmd = connection.CreateCommand();

            cmd.CommandText = $@"SELECT {_tableName}.id, {_tableName}.task_id, action, xp_awarded,  {_tableName}.created_at, user_id FROM {_tableName} 
                                INNER JOIN Task ON {_tableName}.task_id = Task.id
                                WHERE {_tableName}.id = @id";
            cmd.Parameters.AddWithValue("@id", id);

            using var reader = cmd.ExecuteReader();

            if (!reader.Read())
                return null;

            return MapToTaskLog(reader);
        }

        public List<TaskLog> GetAllByUserId(int userId)
        {
            var connection = _dbConnection.GetConnection();
            using var cmd = connection.CreateCommand();

            cmd.CommandText = $@"SELECT {_tableName}.id, {_tableName}.task_id, action, xp_awarded,  {_tableName}.created_at, user_id FROM {_tableName} 
                                INNER JOIN Task ON {_tableName}.task_id = Task.id
                                WHERE user_id = @user_id";
            cmd.Parameters.AddWithValue("@user_id", userId);

            using var reader = cmd.ExecuteReader();

            var taskLog = new List<TaskLog>();
            while (reader.Read())
                taskLog.Add(MapToTaskLog(reader));
            return taskLog;
        }

        public List<TaskLog> GetAllByTaskId(int taskId)
        {
            var connection = _dbConnection.GetConnection();
            using var cmd = connection.CreateCommand();

            cmd.CommandText = $@"SELECT {_tableName}.id, {_tableName}.task_id, action, xp_awarded,  {_tableName}.created_at, user_id FROM {_tableName} 
                                INNER JOIN Task ON {_tableName}.task_id = Task.id
                                WHERE task_id = @task_id";
            cmd.Parameters.AddWithValue("@task_id", taskId);

            using var reader = cmd.ExecuteReader();

            var taskLog = new List<TaskLog>();
            while (reader.Read())
                taskLog.Add(MapToTaskLog(reader));
            return taskLog;
        }

        public override int Insert(TaskLog taskLog)
        {
            var connection = _dbConnection.GetConnection();
            using var cmd = connection.CreateCommand();

            cmd.CommandText = $@"INSERT INTO {_tableName} (task_id, action, xp_awarded) 
                                VALUES (@task_id, @action, @xp_awarded);
                                SELECT last_insert_rowid()";

            cmd.Parameters.AddWithValue("@task_id", taskLog.TaskId);
            cmd.Parameters.AddWithValue("@action", taskLog.Action.ToString());
            cmd.Parameters.AddWithValue("@xp_awarded", taskLog.XpAwarded);

            var id = cmd.ExecuteScalar();

            return Convert.ToInt32(id);
        }

        public int Insert(TaskLog taskLog, SqliteConnection connection, SqliteTransaction transaction)
        {
            using var cmd = connection.CreateCommand();
            cmd.Transaction = transaction;

            cmd.CommandText = $@"INSERT INTO {_tableName} (task_id, action, xp_awarded) 
                                VALUES (@task_id, @action, @xp_awarded);
                                SELECT last_insert_rowid()";

            cmd.Parameters.AddWithValue("@task_id", taskLog.TaskId);
            cmd.Parameters.AddWithValue("@action", taskLog.Action.ToString());
            cmd.Parameters.AddWithValue("@xp_awarded", taskLog.XpAwarded);

            var id = cmd.ExecuteScalar();

            return Convert.ToInt32(id);
        }

        public List<TaskLog> GetRecentByUserId(int userId, int limit)
        {
            var connection = _dbConnection.GetConnection();
            using var cmd = connection.CreateCommand();

            cmd.CommandText = $@"SELECT {_tableName}.id, {_tableName}.task_id, action, xp_awarded, {_tableName}.created_at, user_id FROM {_tableName}
                                INNER JOIN Task ON {_tableName}.task_id = Task.id
                                WHERE Task.user_id = @user_id
                                ORDER BY {_tableName}.created_at DESC
                                LIMIT {limit}";
            cmd.Parameters.AddWithValue("@user_id", userId);

            using var reader = cmd.ExecuteReader();

            var taskLogs = new List<TaskLog>();
            while (reader.Read())
                taskLogs.Add(MapToTaskLog(reader));
            return taskLogs;
        }

        private static TaskLog MapToTaskLog(SqliteDataReader reader)
        {
            TaskLog taskLog = new(
                    reader.GetInt32(reader.GetOrdinal("user_id")),
                    reader.GetInt32(reader.GetOrdinal("task_id")),
                    Enum.Parse<RewardReason>(reader.GetString(reader.GetOrdinal("action"))),
                    reader.GetInt32(reader.GetOrdinal("xp_awarded"))
                );

            taskLog.SetId(reader.GetInt32(reader.GetOrdinal("id")));
            taskLog.SetCreatedAt(reader.GetDateTime(reader.GetOrdinal("created_at")));

            return taskLog;
        }
    }
}