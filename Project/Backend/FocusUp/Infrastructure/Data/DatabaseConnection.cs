using System;
using Microsoft.Data.Sqlite;

public class DatabaseConnection
{
    private static DatabaseConnection _instance;
    private string _connectionString;
    private SqliteConnection _connection;

    private DatabaseConnection(string connectionString)
    {
        _connectionString = connectionString;
        _connection = new SqliteConnection(_connectionString);
    }

    // sets the connection string and sets the sqlite connection
    public static DatabaseConnection GetInstance(string connectionString)
    {
        _instance ??= new DatabaseConnection(connectionString);
        return _instance;
    }

    public void Open() => _connection.Open();

    public void Close() => _connection.Close();

    public SqliteConnection GetConnection()
    {   
        Open();
        return _connection;
    }

    // Execute commands without return values (Insert, Update, Delete)
    public void ExecuteNonQuery(string sql)
    {
        var cmd = _connection.CreateCommand();
        cmd.CommandText = sql;
        cmd.ExecuteNonQuery();
    }

    public void ExecuteNonQuery(string sql, string parameterName, object? parameterValue)
    {
        var cmd = _connection.CreateCommand();
        cmd.CommandText = sql;
        cmd.Parameters.AddWithValue(parameterName, parameterValue);
        cmd.ExecuteNonQuery();
    }

    // Execute commands with one return value or null
    public object? ExecuteScalar(string sql)
    {
        var cmd = _connection.CreateCommand();
        cmd.CommandText = sql;
        return cmd.ExecuteScalar();
    }

    /// <summary>
    /// Execute commands as a scalar function.
    /// </summary>
    /// <param name="sql">sql command</param>
    /// <param name="parameterName">name of the parameter</param>
    /// <param name="parameterValue">value of the parameter</param>
    /// <returns>Returns a object if no return value then null</returns>
    public object? ExecuteScalar(string sql, string parameterName, object? parameterValue)
    {
        var cmd = _connection.CreateCommand();
        cmd.CommandText = sql;
        cmd.Parameters.AddWithValue(parameterName, parameterValue ?? DBNull.Value);

        return cmd.ExecuteScalar();
    }

    public bool TestConnection()
    {
        try
        {
            _connection.Open();

            var cmd = _connection.CreateCommand();
            cmd.CommandText = "SELECT 1";
            cmd.ExecuteScalar();

            _connection.Close();
            return true;
        }
        catch
        {
            return false;
        }
    }
}