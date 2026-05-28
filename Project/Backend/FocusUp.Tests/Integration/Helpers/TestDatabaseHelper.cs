using Microsoft.Data.Sqlite;

namespace FocusUp.Tests;

/// <summary>
/// Provides helper methods for creating,
/// initializing and cleaning the test database.
/// </summary>
public static class TestDatabaseHelper
{
    /// <summary>
    /// Indicates whether the database schema
    /// has already been initialized.
    /// </summary>
    private static bool _initialized;

    /// <summary>
    /// Shared database connection instance
    /// used across all tests.
    /// </summary>
    private static readonly DatabaseConnection _db =
        DatabaseConnection.GetInstance("Data Source=focusup-tests.db");

    /// <summary>
    /// Gets a clean test database instance.
    /// Automatically initializes the schema
    /// on first access.
    /// </summary>
    public static DatabaseConnection Db
    {
        get
        {
            if (!_initialized)
            {
                Initialize();
                _initialized = true;
            }

            Clean();
            return _db;
        }
    }

    /// <summary>
    /// Creates all required database tables
    /// for the test environment.
    /// </summary>
    private static void Initialize()
    {
        var connection = _db.GetConnection();

        using var cmd = connection.CreateCommand();
        cmd.CommandText = """
        PRAGMA foreign_keys = ON;

        CREATE TABLE IF NOT EXISTS User (
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            username VARCHAR(50) NOT NULL UNIQUE,
            email VARCHAR(50) NOT NULL UNIQUE,
            password_hash VARCHAR(255) NOT NULL,
            created_at DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
            updated_at DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP
        );

        CREATE TABLE IF NOT EXISTS Task (
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            user_id INTEGER NOT NULL,
            category_id INTEGER,
            title VARCHAR(255) NOT NULL,
            description VARCHAR(255),
            difficulty VARCHAR(20),
            duration_min INTEGER,
            due_date DATETIME,
            status VARCHAR(20) NOT NULL,
            completed_at DATETIME,
            created_at DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
            updated_at DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
            FOREIGN KEY (user_id) REFERENCES User(id) ON DELETE CASCADE
        );

        CREATE TABLE IF NOT EXISTS TaskLog (
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            task_id INTEGER NOT NULL,
            action VARCHAR(255),
            xp_awarded INTEGER DEFAULT 0,
            created_at DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
            FOREIGN KEY (task_id) REFERENCES Task(id) ON DELETE CASCADE
        );

        CREATE TABLE IF NOT EXISTS XPEvent (
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            user_id INTEGER NOT NULL,
            task_id INTEGER,
            reason VARCHAR(20),
            amount INTEGER NOT NULL,
            created_at DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
            FOREIGN KEY (user_id) REFERENCES User(id) ON DELETE CASCADE,
            FOREIGN KEY (task_id) REFERENCES Task(id) ON DELETE SET NULL
        );

        CREATE TABLE IF NOT EXISTS UserStats (
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            user_id INTEGER NOT NULL UNIQUE,
            total_xp INTEGER DEFAULT 0,
            tasks_done INTEGER DEFAULT 0,
            tasks_open INTEGER DEFAULT 0,
            total_time_min INTEGER DEFAULT 0,
            streak_count INTEGER DEFAULT 0,
            best_streak INTEGER DEFAULT 0,
            streak_last_date DATETIME,
            last_active_at DATETIME,
            created_at DATETIME DEFAULT CURRENT_TIMESTAMP,
            updated_at DATETIME DEFAULT CURRENT_TIMESTAMP,
            FOREIGN KEY (user_id) REFERENCES User(id) ON DELETE CASCADE
        );

        CREATE TABLE IF NOT EXISTS Badge (
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            name VARCHAR(50) NOT NULL UNIQUE,
            description VARCHAR(255),
            rule_type VARCHAR(50) NOT NULL,
            rule_value INTEGER NOT NULL,
            svg TEXT,
            primary_color VARCHAR(7) NOT NULL DEFAULT '#FFFFFF',
            secondary_color VARCHAR(7) NOT NULL DEFAULT '#000000',
            rarity VARCHAR(20) NOT NULL DEFAULT 'Common',
            created_at DATETIME DEFAULT CURRENT_TIMESTAMP
        );

        CREATE TABLE IF NOT EXISTS UserBadge (
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            user_id INTEGER NOT NULL,
            badge_id INTEGER NOT NULL,
            awarded_at DATETIME DEFAULT CURRENT_TIMESTAMP,
            FOREIGN KEY (user_id) REFERENCES User(id) ON DELETE CASCADE,
            FOREIGN KEY (badge_id) REFERENCES Badge(id) ON DELETE CASCADE,
            UNIQUE (user_id, badge_id)
        );
        """;

        cmd.ExecuteNonQuery();
    }

    /// <summary>
    /// Removes all data from database tables
    /// and resets auto-increment counters.
    /// </summary>
    public static void Clean()
    {
        var connection = _db.GetConnection();

        using var cmd = connection.CreateCommand();
        cmd.CommandText = """
        PRAGMA foreign_keys = OFF;

        DELETE FROM UserBadge;
        DELETE FROM Badge;
        DELETE FROM XPEvent;
        DELETE FROM TaskLog;
        DELETE FROM Task;
        DELETE FROM UserStats;
        DELETE FROM User;

        DELETE FROM sqlite_sequence WHERE name IN (
            'UserBadge',
            'Badge',
            'XPEvent',
            'TaskLog',
            'Task',
            'UserStats',
            'User'
        );

        PRAGMA foreign_keys = ON;
        """;

        cmd.ExecuteNonQuery();
    }

    /// <summary>
    /// Inserts a test user into the database.
    /// </summary>
    /// <returns>
    /// The ID of the created user.
    /// </returns>
    public static int InsertUser()
    {
        var unique = Guid.NewGuid().ToString("N");

        var connection = _db.GetConnection();

        using var cmd = connection.CreateCommand();
        cmd.CommandText = """
        INSERT INTO User (username, email, password_hash)
        VALUES (@username, @email, @passwordHash);
        SELECT last_insert_rowid();
        """;

        cmd.Parameters.AddWithValue("@username", $"testuser_{unique}");
        cmd.Parameters.AddWithValue("@email", $"test_{unique}@example.com");
        cmd.Parameters.AddWithValue("@passwordHash", "hash");

        return Convert.ToInt32(cmd.ExecuteScalar());
    }
}