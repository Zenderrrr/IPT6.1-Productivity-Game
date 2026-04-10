using FocusUp.Domain.Enums;
using Microsoft.Data.Sqlite;
using System;
using static FocusUp.Domain.Enums.TaskStatus;

namespace FocusUp.Infrastructure.Repositories
{
    public class TaskRepository : BaseRepository<Task>
    {
        public TaskRepository(DatabaseConnection databaseConnection) : base(databaseConnection, "Task")
        {
        }

        public override Task? GetById(int id)
        {
            var connection = _dbConnection.GetConnection();
            var cmd = connection.CreateCommand();

            cmd.CommandText = $@"SELECT * FROM {_tableName}
                                WHERE id = @id";

            cmd.Parameters.AddWithValue("@id", id);

            using var reader = cmd.ExecuteReader();

            if (!reader.Read())
                return null;

            return MapToTask(reader);
        }

        public List<Task> GetAllByUserId(int userId)
        {
            var connection = _dbConnection.GetConnection();
            var cmd = connection.CreateCommand();

            cmd.CommandText = $@"SELECT * FROM {_tableName}
                                WHERE user_id = @user_id";
            cmd.Parameters.AddWithValue("@user_id", userId);

            using var reader = cmd.ExecuteReader();

            var tasks = new List<Task>();
            while (reader.Read())
                tasks.Add(MapToTask(reader));

            return tasks;
        }

        public List<Task> GetOpenByUserId(int userId)
        {
            var connection = _dbConnection.GetConnection();
            var cmd = connection.CreateCommand();

            cmd.CommandText = $@"SELECT * FROM {_tableName}
                                 WHERE (status = '{Open.ToString()}' OR status = '{InProgress.ToString()}')  AND user_id = @user_id";
            cmd.Parameters.AddWithValue("@user_id", userId);

            using var reader = cmd.ExecuteReader();

            var tasks = new List<Task>();
            while (reader.Read())
                tasks.Add(MapToTask(reader));
            return tasks;
        }

        public List<Task> GetCompletedByUserId(int userId)
        {
            var connection = _dbConnection.GetConnection();
            var cmd = connection.CreateCommand();

            cmd.CommandText = $@"SELECT * FROM {_tableName}
                                 WHERE status = '{Completed.ToString()}' AND user_id = @user_id";
            cmd.Parameters.AddWithValue("@user_id", userId);

            using var reader = cmd.ExecuteReader();

            var tasks = new List<Task>();
            while (reader.Read())
                tasks.Add(MapToTask(reader));
            return tasks;
        }

        public override int Insert(Task task)
        {
            var connection = _dbConnection.GetConnection();
            var cmd = connection.CreateCommand();

            cmd.CommandText = $@"INSERT INTO {_tableName} 
                                 (user_id, category_id, title, description, difficulty, duration_min, due_date, status)
                                 VALUES (@user_id, @category_id, @title, @description, @difficulty, @duration_min, @due_date, @status);
                                 SELECT last_insert_rowid()";
            
            cmd.Parameters.AddWithValue("@user_id", task.UserId);
            cmd.Parameters.AddWithValue("@category_id", task.CategoryId);
            cmd.Parameters.AddWithValue("@title", task.Title);
            cmd.Parameters.AddWithValue("@description", task.Description);
            cmd.Parameters.AddWithValue("@difficulty", task.Difficulty.ToString());
            cmd.Parameters.AddWithValue("@duration_min", task.DurationMin);
            cmd.Parameters.AddWithValue("@due_date", task.DueDate);
            cmd.Parameters.AddWithValue("@status", task.Status.ToString());

            var id = cmd.ExecuteScalar();

            return Convert.ToInt32(id);
        }

        public override void Update(Task task)
        {
            var connection = _dbConnection.GetConnection();
            var cmd = connection.CreateCommand();

            cmd.CommandText = $@"UPDATE {_tableName}
                                 SET category_id = @category_id, title = @title, description = @description, difficulty = @difficulty, duration_min = @duration_min, due_date = @due_date, status = @status, completed_at = @completed_at, updated_at = @updated_at
                                 WHERE id = @id";
            
            cmd.Parameters.AddWithValue("@id", task.Id);
            cmd.Parameters.AddWithValue("@category_id", task.CategoryId);
            cmd.Parameters.AddWithValue("@title", task.Title);
            cmd.Parameters.AddWithValue("@description", task.Description);
            cmd.Parameters.AddWithValue("@difficulty", task.Difficulty.ToString());
            cmd.Parameters.AddWithValue("@duration_min", task.DurationMin);
            cmd.Parameters.AddWithValue("@due_date", task.DueDate);
            cmd.Parameters.AddWithValue("@status", task.Status.ToString());
            cmd.Parameters.AddWithValue("@completed_at", task.CompletedAt);
            cmd.Parameters.AddWithValue("@updated_at", task.UpdatedAt);

            cmd.ExecuteNonQuery();
        }

        public bool Exists(int id)
        {
            var connection = _dbConnection.GetConnection();
            var cmd = connection.CreateCommand();

            cmd.CommandText = $@"SELECT 1 FROM {_tableName} WHERE id = @id";
            cmd.Parameters.AddWithValue("@id", id);

            using var reader = cmd.ExecuteReader();

            if (!reader.Read())
                return false;
            return true;
        }

        public void UpdateStatus(int taskId, Domain.Enums.TaskStatus status, DateTime? completedAt)
        {
            var connection = _dbConnection.GetConnection();
            var cmd = connection.CreateCommand();

            cmd.CommandText = $@"UPDATE {_tableName}
                                 SET status = @status, completed_at = @completed_at, updated_at = @updated_at
                                 WHERE id = @id";

            cmd.Parameters.AddWithValue("@id", taskId);
            cmd.Parameters.AddWithValue("@status", status.ToString());
            cmd.Parameters.AddWithValue("@completed_at", completedAt);
            cmd.Parameters.AddWithValue("@updated_at", DateTime.Now);

            cmd.ExecuteNonQuery();
        }

        public int CountOpenTasks(int userId)
        {
            var connection = _dbConnection.GetConnection();
            var cmd = connection.CreateCommand();

            cmd.CommandText = $@"SELECT COUNT(*) AS 'Count' FROM {_tableName}
                                 WHERE user_id = @user_id AND (status = '{Open.ToString()}' OR status = '{InProgress.ToString()}')";
            cmd.Parameters.AddWithValue("@user_id", userId);

            var count = cmd.ExecuteScalar();

            return Convert.ToInt32(count);
        }

        public int CountCompletedTasks(int userId)
        {
            var connection = _dbConnection.GetConnection();
            var cmd = connection.CreateCommand();

            cmd.CommandText = $@"SELECT COUNT(*) AS 'Count' FROM {_tableName}
                                 WHERE user_id = @user_id AND status = '{Completed.ToString()}'";
            cmd.Parameters.AddWithValue("@user_id", userId);

            var count = cmd.ExecuteScalar();

            return Convert.ToInt32(count);
        }

        public int CountCompletedTasks(int userId, DateTime from, DateTime to)
        {
            var connection = _dbConnection.GetConnection();
            var cmd = connection.CreateCommand();

            cmd.CommandText = $@"SELECT COUNT(*) AS 'Count' FROM {_tableName}
                                 WHERE user_id = @user_id AND status = '{Completed.ToString()}' AND (completed_at BETWEEN @from AND @to)";
            cmd.Parameters.AddWithValue("@user_id", userId);
            cmd.Parameters.AddWithValue("@from", from);
            cmd.Parameters.AddWithValue("@to", to);

            var count = cmd.ExecuteScalar();

            return Convert.ToInt32(count);
        }

        public int SumDurationByPeriod(int userId, DateTime from, DateTime to)
        {
            var connection = _dbConnection.GetConnection();
            var cmd = connection.CreateCommand();

            cmd.CommandText = $@"SELECT SUM(duration_min) FROM {_tableName}
                                 WHERE user_id = @user_id AND status = '{Completed.ToString()}' AND (completed_at BETWEEN @from AND @to)";
            cmd.Parameters.AddWithValue("@user_id", userId);
            cmd.Parameters.AddWithValue("@from", from);
            cmd.Parameters.AddWithValue("@to", to);

            var duration = cmd.ExecuteScalar();

            if(duration == null || duration == DBNull.Value)
                return 0;

            return Convert.ToInt32(duration);
        }

        public List<(DateTime date, int durationMin)> SumDurationPerDay(int userId, DateTime from, DateTime to)
        {
            List<DateOnly> days = new();
            for (int i = 0; i < (to.Date - from.Date).Days + 1; i++)
                days.Add(DateOnly.FromDateTime(from.Date.AddDays(i)));

            List<(DateTime date, int durationMin)> durationPerDayList = new();
            foreach (var day in days)
            {
                int durationMin = SumDurationByPeriod(userId, day.ToDateTime(new TimeOnly(0, 0, 0)), day.ToDateTime(TimeOnly.MaxValue));
                durationPerDayList.Add((day.ToDateTime(new TimeOnly(0, 0, 0)), durationMin));
            }
            return durationPerDayList;
        }

        public List<(DateTime date, int count)> GetCompletedTasksPerDay(int userId, DateTime from, DateTime to)
        {
            List<DateOnly> days = new();
            for (int i = 0; i < (to.Date - from.Date).Days + 1; i++)
                days.Add(DateOnly.FromDateTime(from.Date.AddDays(i)));

            List<(DateTime date, int count)> completedTaskList = new();
            foreach (var day in days)
            {
                int taskCount = CountCompletedTasks(userId, day.ToDateTime(new TimeOnly(0, 0, 0)), day.ToDateTime(TimeOnly.MaxValue));
                completedTaskList.Add((day.ToDateTime(new TimeOnly(0, 0, 0)), taskCount));
            }
            return completedTaskList;
        }

        private static Task MapToTask(SqliteDataReader reader)
        {
            int categoryId = reader.GetOrdinal("category_id");
            int dueDateId = reader.GetOrdinal("due_date");
            int completedAtId = reader.GetOrdinal("completed_at");

            int? categoryIndex = reader.IsDBNull(categoryId) ? null : reader.GetInt32(categoryId);
            DateTime? dueDate = reader.IsDBNull(dueDateId) ? null : reader.GetDateTime(dueDateId);
            DateTime? completedAt = reader.IsDBNull(completedAtId) ? null : reader.GetDateTime(completedAtId);

            Task task = new Task(
                    reader.GetInt32(reader.GetOrdinal("user_id")),
                    reader.GetString(reader.GetOrdinal("title")),
                    reader.GetString(reader.GetOrdinal("description")),
                    Enum.Parse<TaskDifficultyType>(reader.GetString(reader.GetOrdinal("difficulty"))),
                    reader.GetInt32(reader.GetOrdinal("duration_min")),
                    Enum.Parse<Domain.Enums.TaskStatus>(reader.GetString(reader.GetOrdinal("status"))),
                    categoryIndex,
                    dueDate
                );

            task.SetId(reader.GetInt32(reader.GetOrdinal("id")));
            task.SetCreatedAt(reader.GetDateTime(reader.GetOrdinal("created_at")));
            task.SetUpdatedAt(reader.GetDateTime(reader.GetOrdinal("updated_at")));
            task.SetCompletedAt(completedAt);

            return task;
        }
    }
}