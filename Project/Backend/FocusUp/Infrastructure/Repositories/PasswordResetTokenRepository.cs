using FocusUp.Domain.Enums;
using FocusUp.Domain.Models;
using Microsoft.Data.Sqlite;
using System;
using System.Data.Common;

namespace FocusUp.Infrastructure.Repositories
{
    public class PasswordResetTokenRepository : BaseRepository<PasswordResetToken>
    {
        public PasswordResetTokenRepository(DatabaseConnection databaseConnection) : base(databaseConnection, "PasswordResetToken")
        {
        }

        public override PasswordResetToken? GetById(int id)
        {
            var connection = _dbConnection.GetConnection();

            using var cmd = connection.CreateCommand();

            cmd.CommandText = $@"SELECT {_tableName}.id, user_id, token_hash, expires_at, used_at, {_tableName}.created_at FROM {_tableName} 
                                INNER JOIN User ON {_tableName}.user_id = User.id
                                WHERE {_tableName}.id = @id";
            cmd.Parameters.AddWithValue("@id", id);

            using var reader = cmd.ExecuteReader();

            if (!reader.Read())
                return null;

            return MapToPasswordResetToken(reader);
        }

        public override int Insert(PasswordResetToken passwordResetToken)
        {
            using var connection = _dbConnection.CreateOpenConnection();
            using var cmd = connection.CreateCommand();

            DateTime now = DateTime.UtcNow;

            cmd.CommandText = $@"INSERT INTO {_tableName} (user_id, token_hash, created_at, expires_at) 
                                SELECT @user_id, @token_hash, @created_at, @expires_at
                                WHERE NOT EXISTS (
                                    SELECT 1 FROM {_tableName}
                                    WHERE user_id = @user_id AND created_at > @recent
                                )
                                RETURNING id;
                                ";

            cmd.Parameters.AddWithValue("@user_id", passwordResetToken.UserId);
            cmd.Parameters.AddWithValue("@token_hash", passwordResetToken.TokenHash.ToString());
            cmd.Parameters.AddWithValue("@expires_at", passwordResetToken.ExpiresAt);
            cmd.Parameters.AddWithValue("@created_at", now);
            cmd.Parameters.AddWithValue("@recent", now.AddSeconds(-60));

            var id = cmd.ExecuteScalar();

            return id == null || id == DBNull.Value ? 0 : Convert.ToInt32(id);
        }

        public bool TryCompleteReset(string tokenHash, Action<int, DbTransaction> changePassword)
        {
            using var connection = _dbConnection.CreateOpenConnection();
            using var transaction = connection.BeginTransaction();

            DateTime now = DateTime.UtcNow;

            using var claim = connection.CreateCommand();
            claim.Transaction = transaction;
            claim.CommandText = $@"
                                UPDATE {_tableName}
                                SET used_at = @now
                                WHERE token_hash = @token_hash AND used_at = null AND expires_at > @now;
                                ";

            claim.Parameters.AddWithValue("@now", now);
            claim.Parameters.AddWithValue("@token_hash", tokenHash);

            if(claim.ExecuteNonQuery() != 1)
            {
                transaction.Rollback();
                return false;
            }

            using var lookup = connection.CreateCommand();
            lookup.Transaction = transaction;
            lookup.CommandText = $@"
                                    SELECT user_id FROM {_tableName}
                                    WHERE token_hash = @token_hash;
                                    ";

            lookup.Parameters.AddWithValue("@token_hash", tokenHash);

            int userId = Convert.ToInt32(lookup.ExecuteScalar());


            changePassword(userId, transaction);

            using var invalidate = connection.CreateCommand();
            invalidate.Transaction = transaction;
            invalidate.CommandText = $@"
                                    UPDATE {_tableName}
                                    SET used_at = @now
                                    WHERE user_id = @user_id AND used_at IS NULL;
                                    ";

            invalidate.Parameters.AddWithValue("@now", now);
            invalidate.Parameters.AddWithValue("@user_id", userId);

            invalidate.ExecuteNonQuery();

            transaction.Commit();
            return true;
        }

        private static PasswordResetToken MapToPasswordResetToken(SqliteDataReader reader)
        {
            int usedAtOrdinal = reader.GetOrdinal("used_at");

            DateTime? usedAt = reader.IsDBNull(usedAtOrdinal) ? null : reader.GetDateTime(usedAtOrdinal);

            PasswordResetToken passwordResetToken = new(
                    reader.GetInt32(reader.GetOrdinal("user_id")),
                    reader.GetString(reader.GetOrdinal("token_hash")),
                    reader.GetDateTime(reader.GetOrdinal("expires_at")),
                    usedAt
                );

            passwordResetToken.SetId(reader.GetInt32(reader.GetOrdinal("id")));
            passwordResetToken.SetCreatedAt(reader.GetDateTime(reader.GetOrdinal("created_at")));

            return passwordResetToken;
        }
    }
}