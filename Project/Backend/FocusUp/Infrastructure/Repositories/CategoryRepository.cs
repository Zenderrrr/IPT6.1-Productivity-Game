using Microsoft.Data.Sqlite;
using System;

namespace FocusUp.Infrastructure.Repositories
{
    public class CategoryRepository : BaseRepository<Category>
    {
        public CategoryRepository(DatabaseConnection databaseConnection) : base(databaseConnection, "Category")
        {
        }

        public override Category? GetById(int id)
        {
            using var connection = _dbConnection.GetConnection();
            using var cmd = connection.CreateCommand();

            cmd.CommandText = $@"SELECT * FROM {_tableName}
                                 WHERE id = @id";
            cmd.Parameters.AddWithValue("@id", id);

            using var reader = cmd.ExecuteReader();

            if (!reader.Read())
                return null;

            return MapToCategory(reader);
        }

        public List<Category> GetAllByUserId(int userId)
        {
            using var connection = _dbConnection.GetConnection();
            using var cmd = connection.CreateCommand();

            cmd.CommandText = $@"SELECT * FROM {_tableName}
                                 WHERE user_id = @user_id";
            cmd.Parameters.AddWithValue("@user_id", userId);

            using var reader = cmd.ExecuteReader();

            var category = new List<Category>();
            while (reader.Read())
                category.Add(MapToCategory(reader));
            return category;
        }

        public override int Insert(Category category)
        {
            using var connection = _dbConnection.GetConnection();
            using var cmd = connection.CreateCommand();

            cmd.CommandText = $@"INSERT INTO {_tableName} (user_id, name, color)
                                VALUES (@user_id, @name, @color);
                                SELECT last_insert_rowid()";
            cmd.Parameters.AddWithValue("@user_id", category.UserId);
            cmd.Parameters.AddWithValue("@name", category.Name);
            cmd.Parameters.AddWithValue("@color", category.Color);

            var id = cmd.ExecuteScalar();

            return Convert.ToInt32(id);
        }

        public override void Update(Category category)
        {
            using var connection = _dbConnection.GetConnection();
            using var cmd = connection.CreateCommand();

            cmd.CommandText = $@"UPDATE {_tableName}
                                 SET name = @name, color = @color
                                 WHERE id = @id";
            cmd.Parameters.AddWithValue("@name", category.Name);
            cmd.Parameters.AddWithValue("@color", category.Color);
            cmd.Parameters.AddWithValue("@id", category.Id);

            cmd.ExecuteNonQuery();
        }

        public override void Delete(int id)
        {
            using var connection = _dbConnection.GetConnection();
            using var cmd = connection.CreateCommand();

            cmd.CommandText = $@"DELETE FROM {_tableName} WHERE id = @id";
            cmd.Parameters.AddWithValue("@id", id);

            cmd.ExecuteNonQuery();
        }

        public bool Exists(int id)
        {
            using var connection = _dbConnection.GetConnection();
            using var cmd = connection.CreateCommand();

            cmd.CommandText = $@"SELECT 1 FROM {_tableName}
                                 WHERE id = @id";
            cmd.Parameters.AddWithValue("@id", id);

            using var reader = cmd.ExecuteReader();

            if (!reader.Read())
                return false;
            return true;
        }

        public bool ExistsByName(int userId, string name)
        {
            using var connection = _dbConnection.GetConnection();
            using var cmd = connection.CreateCommand();

            cmd.CommandText = $@"SELECT 1 FROM {_tableName}
                                 WHERE name = @name AND user_id = @user_id";
            cmd.Parameters.AddWithValue("@name", name);
            cmd.Parameters.AddWithValue("@user_id", userId);

            using var reader = cmd.ExecuteReader();

            if (!reader.Read())
                return false;
            return true;
        }

        private static Category MapToCategory(SqliteDataReader reader)
        {
            int colorId = reader.GetOrdinal("color");

            string? color = reader.IsDBNull(colorId) ? null : reader.GetString(colorId);

            var category = new Category(
                    reader.GetInt32(reader.GetOrdinal("user_id")),
                    reader.GetString(reader.GetOrdinal("name")),
                    color
                );

            category.SetId(reader.GetInt32(reader.GetOrdinal("id")));
            category.SetCreatedAt(reader.GetDateTime(reader.GetOrdinal("created_at")));
            return category;
        }
    }
}