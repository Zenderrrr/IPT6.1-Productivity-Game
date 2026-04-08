using FocusUp.Infrastructure.Repositories;
using System;

namespace FocusUp.Application.Services
{
    public class UserStatsService
    {
        private readonly UserStatsRepository _userStatsRepository;

        public UserStatsService(UserStatsRepository userStatsRepository) => _userStatsRepository = userStatsRepository;

        public UserStats? GetUserStats(int userId) => _userStatsRepository.GetByUserId(userId);

        public void UpdateLastactive(int userId) => _userStatsRepository.UpdateLastActive(userId, DateTime.Now);

        public int InitializeUserStats(int userId)
        {
            var userStats = new UserStats(userId);
            return _userStatsRepository.Insert(userStats);
        }
    }
}