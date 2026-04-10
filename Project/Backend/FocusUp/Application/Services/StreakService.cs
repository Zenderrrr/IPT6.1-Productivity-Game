using FocusUp.Application.Strategies.Interfaces;
using FocusUp.Infrastructure.Repositories;
using System;
using FocusUp.Common.Exceptions;

namespace FocusUp.Application.Services
{
    public class StreakService
    {
        private readonly UserStatsRepository _userStatsRepository;
        private readonly IStreakRuleStrategy _streakStrategy;

        public StreakService(UserStatsRepository userStatsRepository, IStreakRuleStrategy streakStrategy)
        {
            _userStatsRepository = userStatsRepository;
            _streakStrategy = streakStrategy;
        }

        public void UpdateStreak(int userId, DateTime completedAt)
        {
            var userStats = _userStatsRepository.GetByUserId(userId) ?? throw new UserStatsNotFoundException(userId);

            if (_streakStrategy.ShouldResetStreak(userStats, completedAt))
                userStats.ResetStreak();
            userStats.IncrementStreak(completedAt);

            _userStatsRepository.UpdateStreak(userId, userStats.StreakCount, userStats.BestStreak, completedAt);
        }

        public int CalculateNewStreak(UserStats userStats, DateTime completedAt)
        {
            if (_streakStrategy.ShouldResetStreak(userStats, completedAt))
                return 1;

            if (userStats.StreakLastDate?.Date == completedAt)
                return userStats.StreakCount;

            return userStats.StreakCount + 1;
        }

        public int GetCurrentStreak(int userId)
        {
            var userStats = _userStatsRepository.GetByUserId(userId) ?? throw new UserStatsNotFoundException(userId);
            return userStats.StreakCount;
        }

        public int GetBestStreak(int userId)
        {
            var userStats = _userStatsRepository.GetByUserId(userId) ?? throw new UserStatsNotFoundException(userId);
            return userStats.BestStreak;
        }
    }
}