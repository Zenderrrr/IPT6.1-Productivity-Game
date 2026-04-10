using FocusUp.Domain.Enums;
using Microsoft.Data.Sqlite;
using System;

namespace FocusUp.Infrastructure.Repositories
{
    public class BadgeRepository : BaseRepository<Badge>
    {
        public BadgeRepository(DatabaseConnection databaseConnection) : base(databaseConnection, "Badge")
        {
        }

        public override Badge? GetById(int id)
        {
            var connection = _dbConnection.GetConnection();

            using var cmd = connection.CreateCommand();

            cmd.CommandText = $"SELECT * FROM {_tableName} WHERE id = @id";
            cmd.Parameters.AddWithValue("@id", id);

            using var reader = cmd.ExecuteReader();

            if(!reader.Read())
                return null;

            return MapToBadge(reader);
        }

        public List<Badge> GetAll()
        {
            var connection = _dbConnection.GetConnection();
            using var cmd = connection.CreateCommand();

            cmd.CommandText = $"SELECT * FROM {_tableName}";

            using var reader = cmd.ExecuteReader();

            var badges = new List<Badge>();
            while (reader.Read())
                badges.Add(MapToBadge(reader));
            return badges;
        }

        public List<Badge> GetByRuleType(BadgeRuleType ruleType)
        {
            var connection = _dbConnection.GetConnection();

            using var cmd = connection.CreateCommand();

            cmd.CommandText = $"SELECT * FROM {_tableName} WHERE rule_type = @ruleType";
            cmd.Parameters.AddWithValue("@ruleType", ruleType.ToString());

            using var reader = cmd.ExecuteReader();

            var badges = new List<Badge>();
            while (reader.Read())
                badges.Add(MapToBadge(reader));
            return badges;
        }

        public override int Insert(Badge badge)
        {
            var connection = _dbConnection.GetConnection();
            using var cmd = connection.CreateCommand();

            cmd.CommandText = $@"INSERT INTO {_tableName} (name, description, rule_type, rule_value, created_at) 
                                VALUES (@name, @description, @rule_type, @rule_value, @created_at);
                                SELECT last_insert_rowid()";

            cmd.Parameters.AddWithValue("@name", badge.Name);
            cmd.Parameters.AddWithValue("@description", badge.Description);
            cmd.Parameters.AddWithValue("@rule_type", badge.RuleType.ToString());
            cmd.Parameters.AddWithValue("@rule_value", badge.RuleValue);
            cmd.Parameters.AddWithValue("@created_at", badge.CreatedAt);

            var id = cmd.ExecuteScalar();

            return Convert.ToInt32(id);
        }

        public override void Update(Badge badge)
        {
            var connection = _dbConnection.GetConnection();
            using var cmd = connection.CreateCommand();

            cmd.CommandText = $@"UPDATE {_tableName}
                                SET name = @name, description = @description, rule_type = @rule_type, rule_value = @rule_value
                                WHERE id = @id";
            cmd.Parameters.AddWithValue("@id", badge.Id);
            cmd.Parameters.AddWithValue("@name", badge.Name);
            cmd.Parameters.AddWithValue("@description", badge.Description);
            cmd.Parameters.AddWithValue("@rule_type", badge.RuleType.ToString());
            cmd.Parameters.AddWithValue("@rule_value", badge.RuleValue);

            cmd.ExecuteNonQuery();
        }

        public bool ExistsByName(string name)
        {
            var connection = _dbConnection.GetConnection();
            using var cmd = connection.CreateCommand();

            cmd.CommandText = $"SELECT 1 FROM {_tableName} WHERE name = @name";
            cmd.Parameters.AddWithValue ("@name", name);

            using var reader = cmd.ExecuteReader();

            if(reader.Read())
                return true;
            return false;
        }

        private static Badge MapToBadge(SqliteDataReader reader)
        {
            int descriptionId = reader.GetOrdinal("description");
            string? description = reader.IsDBNull(descriptionId) ? null : reader.GetString(descriptionId);

            Badge badge = new Badge(
                reader.GetString(reader.GetOrdinal("name")),
                description ?? "",
                Enum.Parse<BadgeRuleType>(reader.GetString(reader.GetOrdinal("rule_type"))),
                reader.GetInt32(reader.GetOrdinal("rule_value"))
                );

            badge.SetId(reader.GetInt32(reader.GetOrdinal("id")));
            badge.SetCreatedAt(DateTime.Parse(reader["created_at"].ToString()!));

            return badge;
        }
    }
}