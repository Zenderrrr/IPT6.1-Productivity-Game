using FocusUp.Application.Strategies.Interfaces;
using System;

namespace FocusUp.Application.Strategies
{
    public class DefaultStreakRuleStrategy : IStreakRuleStrategy
    {
        private readonly int _maxGapDays;
        
        public DefaultStreakRuleStrategy(int maxGapDays = 1) => _maxGapDays = maxGapDays;

        public int CalculateNewStreak(UserStats stats, DateTime completedAt)
        {
            if(ShouldResetStreak(stats, completedAt))
                stats.ResetStreak();
            else
                stats.IncrementStreak(completedAt);
            return stats.StreakCount;
        }
        public bool ShouldResetStreak(UserStats stats, DateTime today)
        {
            return stats.ShouldResetStreak(today, _maxGapDays);
        }
    }
}