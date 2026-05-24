using FocusUp.Application.Strategies.Interfaces;
using FocusUp.Domain.Enums;
using System;

namespace FocusUp.Application.Strategies
{
    public class TotalXpBadgeRule : IBadgeRule
    {
        private readonly BadgeRuleType _ruleType = BadgeRuleType.xp_total;

        public TotalXpBadgeRule() { }

        public bool IsUnlocked(BadgeContext context, Badge badge) => context.Stats.TotalXp >= badge.RuleValue;

        public BadgeRuleType GetRuleType() => _ruleType;
    }
}