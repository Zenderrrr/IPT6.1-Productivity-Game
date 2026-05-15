using FocusUp.Application.Strategies.Interfaces;
using FocusUp.Domain.Enums;
using System;

namespace FocusUp.Application.Strategies
{
    public class LongTaskDurationBadgeRule : IBadgeRule
    {
        private readonly BadgeRuleType _ruleType = BadgeRuleType.long_task_duration;

        public LongTaskDurationBadgeRule() { }

        public bool IsUnlocked(UserStats stats, Badge badge)
        {
            throw new NotImplementedException();
        }

        public BadgeRuleType GetRuleType() => _ruleType;
    }
}