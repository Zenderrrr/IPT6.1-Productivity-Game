using Microsoft.Data.Sqlite;
using System;

namespace FocusUp.Infrastructure.Repositories
{
    public class UserRepository : BaseRepository<User>
    {
        public UserRepository(DatabaseConnection databaseConnection) : base(databaseConnection, "User")
        {
        }

        public override User? GetById(int id)
        {
            var connection = _dbConnection.GetConnection();
            var cmd = connection.CreateCommand();

            cmd.CommandText = $@"SELECT * FROM {_tableName}
                                 WHERE id = @id";
            cmd.Parameters.AddWithValue("@id", id);

            using var reader = cmd.ExecuteReader();

            if(!reader.Read())
                return null;
            return MapToUser(reader);
        }

        public User? GetByUsername(string username)
        {
            var connection = _dbConnection.GetConnection();
            var cmd = connection.CreateCommand();

            cmd.CommandText = $@"SELECT * FROM {_tableName}
                                 WHERE username = @username";

            cmd.Parameters.AddWithValue("@username", username);

            using var reader = cmd.ExecuteReader();

            if (!reader.Read())
                return null;
            return MapToUser(reader);
        }

        public User? GetByEmail(string email)
        {
            var connection = _dbConnection.GetConnection();
            var cmd = connection.CreateCommand();

            cmd.CommandText = $@"SELECT * FROM {_tableName}
                                 WHERE email = @email";

            cmd.Parameters.AddWithValue("@email", email);

            using var reader = cmd.ExecuteReader();

            if (!reader.Read())
                return null;
            return MapToUser(reader);
        }

        public override int Insert(User user)
        {
            var connection = _dbConnection.GetConnection();
            var cmd = connection.CreateCommand();

            cmd.CommandText = $@"INSERT INTO {_tableName} (username, email, password_hash)
                                 VALUES (@username, @email, @password_hash);
                                 SELECT last_insert_rowid()";

            cmd.Parameters.AddWithValue("@username", user.Username);
            cmd.Parameters.AddWithValue("@email", user.Email);
            cmd.Parameters.AddWithValue("@password_hash", user.PasswordHash);

            var id = cmd.ExecuteScalar();

            return Convert.ToInt32(id);
        }

        public override void Update(User user)
        {
            var connection = _dbConnection.GetConnection();
            var cmd = connection.CreateCommand();

            cmd.CommandText = $@"UPDATE {_tableName}
                                 SET username = @username, email = @email, password_hash = @password_hash, updated_at = @updated_at
                                 WHERE id = @id";

            cmd.Parameters.AddWithValue("@id", user.Id);
            cmd.Parameters.AddWithValue("@username", user.Username);
            cmd.Parameters.AddWithValue("@email", user.Email);
            cmd.Parameters.AddWithValue("@password_hash", user.PasswordHash);
            cmd.Parameters.AddWithValue("@updated_at", user.UpdatedAt);

            cmd.ExecuteNonQuery();
        }

        public bool ExistsByUsername(string username)
        {
            var connection = _dbConnection.GetConnection();
            var cmd = connection.CreateCommand();

            cmd.CommandText = $@"SELECT 1 FROM {_tableName}
                                 WHERE username = @username";

            cmd.Parameters.AddWithValue("@username", username);

            using var reader = cmd.ExecuteReader();

            if (!reader.Read())
                return false;
            return true;
        }

        public bool ExistsByEmail(string email)
        {
            var connection = _dbConnection.GetConnection();
            var cmd = connection.CreateCommand();

            cmd.CommandText = $@"SELECT 1 FROM {_tableName}
                                 WHERE email = @email";

            cmd.Parameters.AddWithValue("@email", email);

            using var reader = cmd.ExecuteReader();

            if (!reader.Read())
                return false;
            return true;
        }

        public void UpdatePassword(int userId, string passwordHash)
        {
            var connection = _dbConnection.GetConnection();
            var cmd = connection.CreateCommand();

            cmd.CommandText = $@"UPDATE {_tableName}
                                 SET password_hash = @password_hash, updated_at = @updated_at
                                 WHERE id = @id";

            cmd.Parameters.AddWithValue("@password_hash", passwordHash);
            cmd.Parameters.AddWithValue("@updated_at", DateTime.Now);
            cmd.Parameters.AddWithValue("@id", userId);

            cmd.ExecuteNonQuery();
        }

        private static User MapToUser(SqliteDataReader reader)
        {
            User user = new User(
                    reader.GetString(reader.GetOrdinal("username")),
                    reader.GetString(reader.GetOrdinal("email")),
                    reader.GetString(reader.GetOrdinal("password_hash"))
                );

            user.SetId(reader.GetInt32(reader.GetOrdinal("id")));
            user.SetCreatedAt(reader.GetDateTime(reader.GetOrdinal("created_at")));
            user.SetUpdatedAt(reader.GetDateTime(reader.GetOrdinal("updated_at")));

            return user;
        }
    }
}