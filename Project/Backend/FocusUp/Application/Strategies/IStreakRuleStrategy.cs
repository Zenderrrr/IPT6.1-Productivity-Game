using System;

public interface IStreakRuleStrategy
{
    int CalculateNewStreak(UserStats stats, DateTime completedAt);
    bool ShouldResetStreak(UserStats stats, DateTime today);
}