using System;

public class UserStatsNotFoundException : Exception
{
    public UserStatsNotFoundException(int userId) : base($"Userstats from User ID {userId} was not found.")
    {
    }
}