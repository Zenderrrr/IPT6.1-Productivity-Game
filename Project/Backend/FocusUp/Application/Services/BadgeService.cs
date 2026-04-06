using FocusUp.Application.Strategies.Interfaces;
using FocusUp.Common.Exceptions;
using FocusUp.Domain.Enums;
using FocusUp.Infrastructure.Repositories;
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
            DateTime awardedAt = DateTime.Now;

            var badges = GetAvailableBadges();
            UserStats? userStats = _userStatsRepository.GetByUserId(userId) ?? throw new UserStatsNotFoundException(userId);

            List<Badge> awardedBadges = new();

            var userBadges = _userBadgeRepository.GetByUserId(userId);
            foreach (var badge in badges)
            {
                BadgeRuleType badgeRuleType = badge.RuleType;

                bool hasBadge = HasBadge(userBadges, badge.Id);
                if (hasBadge) continue;

                var rule = _badgeRules.FirstOrDefault(b => b.GetRuleType() == badgeRuleType) ?? throw new BadgeRuleNotFoundException(badgeRuleType, badge.Id);
                bool isUnlocked = rule.IsUnlocked(userStats, badge);
                if (!isUnlocked) continue;

                UserBadge userBadge = new UserBadge(userId, badge.Id, awardedAt);
                _userBadgeRepository.Insert(userBadge);

                awardedBadges.Add(badge);
            }
            return awardedBadges;
        }

        public bool HasBadge(List<UserBadge> userBadges, int badgeId) => userBadges.Any(u => u.BadgeId == badgeId);

        public bool HasBadge(int userId, int badgeId)
        {
            var userBadges = _userBadgeRepository.GetByUserId(userId);
            return userBadges.Any(u => u.BadgeId == badgeId);
        }

        public List<Badge> GetUnlockedBadges(int userId)
        {
            var badges = GetAvailableBadges();
            List<Badge> awardedBadges = new();

            var userBadges = _userBadgeRepository.GetByUserId(userId);
            foreach (var badge in badges)
            {
                bool hasBadge = HasBadge(userBadges, badge.Id);
                if (!hasBadge) continue;
                awardedBadges.Add(badge);
            }
            return awardedBadges;
        }

        public List<Badge> GetAvailableBadges() => _badgeRepository.GetAll();
    }
}