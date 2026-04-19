using FocusUp.Domain.Models;
using Microsoft.Data.Sqlite;
using System;

namespace FocusUp.Infrastructure.Repositories
{
    public class UserRefreshTokenRepository : BaseRepository<UserRefreshToken>
    {
        public UserRefreshTokenRepository(DatabaseConnection databaseConnection) : base(databaseConnection, "UserRefreshToken")
        {
        }

        public override int Insert(UserRefreshToken userRefreshToken)
        {
            var connection = _dbConnection.GetConnection();
            using var cmd = connection.CreateCommand();

            cmd.CommandText = $@"INSERT INTO {_tableName} (user_id, token_hash, expires_at)
                                VALUES (@user_id, @token_hash, @expires_at);
                                SELECT last_insert_rowid();";

            cmd.Parameters.AddWithValue("@user_id", userRefreshToken.UserId);
            cmd.Parameters.AddWithValue("@token_hash", userRefreshToken.TokenHash);
            cmd.Parameters.AddWithValue("@expires_at", userRefreshToken.ExpiresAt);

            var id = cmd.ExecuteScalar();

            return Convert.ToInt32(id);
        }

        public void UpdateRevokedAt(UserRefreshToken userRefreshToken)
        {
            var connection = _dbConnection.GetConnection();
            using var cmd = connection.CreateCommand();

            cmd.CommandText = $@"UPDATE {_tableName}
                                 SET revoked_at = @revoked_at
                                 WHERE id = @id";
            cmd.Parameters.AddWithValue("@revoked_at", (object?)userRefreshToken.RevokedAt ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@id", userRefreshToken.Id);

            cmd.ExecuteNonQuery();
        }

        public override UserRefreshToken? GetById(int id)
        {
            var connection = _dbConnection.GetConnection();
            using var cmd = connection.CreateCommand();

            cmd.CommandText = $@"SELECT * FROM {_tableName}
                                WHERE id = @id";

            cmd.Parameters.AddWithValue("@id", id);

            using var reader = cmd.ExecuteReader();

            if (!reader.Read())
                return null;
            return MapToUserRefreshToken(reader);
        }

        public UserRefreshToken? GetByToken(string refreshTokenHash)
        {
            var connection = _dbConnection.GetConnection();
            using var cmd = connection.CreateCommand();

            cmd.CommandText = $@"SELECT * FROM {_tableName}
                                 WHERE token_hash = @token_hash";

            cmd.Parameters.AddWithValue("@token_hash", refreshTokenHash);

            using var reader = cmd.ExecuteReader();

            if (!reader.Read())
                return null;
            return MapToUserRefreshToken(reader);
        }

        public UserRefreshToken MapToUserRefreshToken(SqliteDataReader reader)
        {
            int revokedAtInd = reader.GetOrdinal("revoked_at");

            DateTime? revokedAt = reader.IsDBNull(revokedAtInd) ? null : reader.GetDateTime(revokedAtInd);

            var userRefreshToken = new UserRefreshToken(
                    reader.GetInt32(reader.GetOrdinal("id")),
                    reader.GetInt32(reader.GetOrdinal("user_id")),
                    reader.GetString(reader.GetOrdinal("token_hash")),
                    reader.GetDateTime(reader.GetOrdinal("expires_at")),
                    reader.GetDateTime(reader.GetOrdinal("created_at")),
                    revokedAt
                );
            return userRefreshToken;
        }
    }
}