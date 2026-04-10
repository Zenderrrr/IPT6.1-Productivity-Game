using FocusUp.Application.Strategies.Interfaces;
using FocusUp.Domain.Enums;
using System;

namespace FocusUp.Application.Strategies
{
    public class TimeLoggedBadgeRule : IBadgeRule
    {
        private readonly BadgeRuleType _ruleType = BadgeRuleType.time_logged;

        public TimeLoggedBadgeRule() { }

        public bool IsUnlocked(UserStats stats, Badge badge) => stats.TotalTimeMin >= badge.RuleValue;

        public BadgeRuleType GetRuleType() => _ruleType;
    }
}