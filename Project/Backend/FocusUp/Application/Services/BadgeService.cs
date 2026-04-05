using FocusUp.Application.Strategies.Interfaces;
using System;

namespace FocusUp.Application.Services
{
    public class BadgeService
    {
        private readonly UserStatsRepository _userStatsRepository;
        private readonly BadgeRepository _badgeRepository;
        private readonly UserBadgeRepository _userBadgeRepository;
        private readonly List<IBadgeRule> _badgeRules;

        public BadgeService(UserStatsRepository userStatsRepository, BadgeRepository badgeRepository, UserBadgeRepository userBadgeRepository, List<IBadgeRule> badgeRules)
        {
            _userStatsRepository = userStatsRepository;
            _userBadgeRepository = userBadgeRepository;
            _badgeRepository = badgeRepository;
            _badgeRules = badgeRules;
        }

        public List<Badge> CheckAndAwardBadges(int userId)
        {
            throw new NotImplementedException();
        }

        public bool HasBadge(int userId, int badgeId)
        {
            throw new NotImplementedException();
        }

        public List<Badge> GetUnlockedBadges(int userId)
        {
            throw new NotImplementedException();
        }

        public List<Badge> GetAvailableBadges()
        {
            throw new NotImplementedException();
        }
    }
}