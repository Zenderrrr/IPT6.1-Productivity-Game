using FocusUp.Application.Strategies.Interfaces;
using FocusUp.Domain.Enums;
using System;

namespace FocusUp.Application.Strategies
{
    public class ConsistencyDeadlineBadgeRule : IBadgeRule
    {
        private readonly BadgeRuleType _ruleType = BadgeRuleType.consistency_check_deadline;

        public ConsistencyDeadlineBadgeRule()
        {
        }

        public bool IsUnlocked(BadgeContext context, Badge badge)
        {
            return context.Tasks.Any(t =>
            {
                if (t.CompletedAt == null || t.DueDate == null) return false;

                int minutesBeforeDueDate = (int)((t.DueDate.Value - t.CompletedAt.Value).TotalMinutes);
                return minutesBeforeDueDate <= badge.RuleValue && minutesBeforeDueDate >= 0;
            });
        }

        public BadgeRuleType GetRuleType() => _ruleType;
    }
}