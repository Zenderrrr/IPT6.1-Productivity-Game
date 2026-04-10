using FocusUp.Application.Strategies.Interfaces;
using FocusUp.Domain.Enums;
using System;

namespace FocusUp.Application.Strategies
{
    public class StreakBadgeRule : IBadgeRule
    {
        private readonly BadgeRuleType _ruleType = BadgeRuleType.streak;

        public StreakBadgeRule() { }

        public bool IsUnlocked(UserStats stats, Badge badge) => stats.StreakCount >= badge.RuleValue;

        public BadgeRuleType GetRuleType() => _ruleType;
    }
}