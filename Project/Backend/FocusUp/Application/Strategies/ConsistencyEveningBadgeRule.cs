using FocusUp.Application.Strategies.Interfaces;
using FocusUp.Domain.Enums;
using System;

namespace FocusUp.Application.Strategies
{
    public class ConsistencyEveningBadgeRule : IBadgeRule
    {
        private readonly BadgeRuleType _ruleType = BadgeRuleType.consistency_check_evening;

        public ConsistencyEveningBadgeRule()
        {
        }

        public bool IsUnlocked(BadgeContext context, Badge badge)
        {
            return context.Tasks.Any(t => {
                if (t.CompletedAt == null) return false;

                return new TimeOnly(t.CompletedAt.Value.Hour, t.CompletedAt.Value.Minute) >= new TimeOnly(badge.RuleValue, 0);
            });
        }

        public BadgeRuleType GetRuleType() => _ruleType;
    }
}