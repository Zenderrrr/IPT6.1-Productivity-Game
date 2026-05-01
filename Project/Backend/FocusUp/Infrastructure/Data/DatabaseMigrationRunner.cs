using Microsoft.Data.Sqlite;
using System;

namespace FocusUp.Infrastructure.Data
{
    public class DatabaseMigrationRunner
    {
        private readonly DatabaseConnection _databaseConnection;
        private readonly string _scriptPath;

        public DatabaseMigrationRunner(DatabaseConnection databaseConnection)
        {
            _databaseConnection = databaseConnection;

            _scriptPath = Path.Combine(AppContext.BaseDirectory, "../../Database/");
        }

        public void Run()
        {
            var connection = _databaseConnection.GetConnection();

            EnableForeignKeys(connection);
            CreateMigrationTable(connection);

            RunCreateScriptIfDatabaseIsEmpty(connection);
            RunMigrationScript(connection);
        }

        private void EnableForeignKeys(SqliteConnection connection)
        {
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "PRAGMA foreign_keys = ON;";
            cmd.ExecuteNonQuery();
        }

        private void CreateMigrationTable(SqliteConnection connection)
        {
            using var cmd = connection.CreateCommand();

            cmd.CommandText = @$"
                CREATE TABLE IF NOT EXISTS SchemaMigrations (
                    id INTEGER PRIMARY KEY AUTOINCREMENT,
                    script_name VARCHAR(100) NOT NULL UNIQUE,
                    executed_at DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP
                );
            ";

            cmd.ExecuteNonQuery();
        }

        private void RunCreateScriptIfDatabaseIsEmpty(SqliteConnection connection)
        {
            using var cmd = connection.CreateCommand();

            cmd.CommandText = @$"
                SELECT COUNT(*)
                FROM sqlite_master
                WHERE type = 'table'
                AND name NOT IN ('SchemaMigrations', 'sqlite_sequence')
            ";

            var tableCount = Convert.ToInt32(cmd.ExecuteScalar());

            if (tableCount > 0)
                return;

            var createFile = Path.Combine(_scriptPath, "create.sql");

            if (!File.Exists(createFile))
                return;

            ExecuteSqlFile(connection, createFile);

            MarkScriptAsExecuted(connection, "create.sql");
        }

        private void RunMigrationScript(SqliteConnection connection)
        {
            var migrationsFolder = Path.Combine(_scriptPath, "migrations");

            if (!Directory.Exists(migrationsFolder))
                return;

            var migrationFiles = Directory.GetFiles(migrationsFolder, "*.sql").OrderBy(file => file).ToList();

            foreach (var file in migrationFiles)
            {
                var scriptName = Path.GetFileName(file);

                if (WasScriptAlreadyExecuted(connection, scriptName))
                    continue;

                ExecuteSqlFile(connection, file);
                MarkScriptAsExecuted(connection, scriptName);
            }
        }

        private bool WasScriptAlreadyExecuted(SqliteConnection connection, string scriptName)
        {
            using var cmd = connection.CreateCommand();
            cmd.CommandText = @$"
                    SELECT COUNT(*)
                    FROM SchemaMigrations
                    WHERE script_name = @script_name
                ";
            cmd.Parameters.AddWithValue("@script_name", scriptName);

            var result = cmd.ExecuteScalar();

            return Convert.ToInt32(result) > 0;
        }

        private void ExecuteSqlFile(SqliteConnection connection, string filePath)
        {
            var sql = File.ReadAllText(filePath);

            using var transaction = connection.BeginTransaction();

            try
            {
                using var cmd = connection.CreateCommand();
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();

                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        private void MarkScriptAsExecuted(SqliteConnection connection, string scriptName)
        {
            using var cmd = connection.CreateCommand();
            cmd.CommandText = @$"
                    INSERT INTO SchemaMigrations (script_name)
                    VALUES (@script_name)
                ";

            cmd.Parameters.AddWithValue("@script_name", scriptName);

            cmd.ExecuteNonQuery();
        }
    }
}