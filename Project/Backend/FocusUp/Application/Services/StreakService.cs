using FocusUp.Application.Strategies.Interfaces;
using FocusUp.Infrastructure.Repositories;
using System;

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
            throw new NotImplementedException();
        }

        public void ResetStreakNeeded(int userId, DateTime completedAt)
        {
            throw new NotImplementedException();
        }

        public int GetCurrentStreak(int userId)
        {
            throw new NotImplementedException();
        }

        public int GetBestStreak(int userId)
        {
            throw new NotImplementedException();
        }
    }
}