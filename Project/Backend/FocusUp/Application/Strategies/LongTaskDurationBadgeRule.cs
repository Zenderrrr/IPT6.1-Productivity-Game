using FocusUp.Application.Strategies.Interfaces;
using FocusUp.Domain.Enums;
using System;

namespace FocusUp.Application.Strategies
{
    public class LongTaskDurationBadgeRule : IBadgeRule
    {
        private readonly BadgeRuleType _ruleType = BadgeRuleType.long_task_duration;

        public LongTaskDurationBadgeRule() { }

        public bool IsUnlocked(BadgeContext context, Badge badge)
        {
            return context.Tasks.Any(t => t.DurationMin >= badge.RuleValue);
        }

        public BadgeRuleType GetRuleType() => _ruleType;
    }
}