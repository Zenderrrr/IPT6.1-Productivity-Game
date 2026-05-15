using FocusUp.Application.Strategies.Interfaces;
using FocusUp.Domain.Enums;
using System;

namespace FocusUp.Application.Strategies
{
    public class ConsistencyEveningBadgeRule : IBadgeRule
    {
        private readonly BadgeRuleType _ruleType = BadgeRuleType.consistency_check_morning;

        public ConsistencyEveningBadgeRule()
        {
        }

        public bool IsUnlocked(UserStats stats, Badge badge)
        {
            throw new NotImplementedException();
        }

        public BadgeRuleType GetRuleType() => _ruleType;
    }
}