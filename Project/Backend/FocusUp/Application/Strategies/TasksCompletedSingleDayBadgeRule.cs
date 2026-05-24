using FocusUp.Application.Strategies.Interfaces;
using FocusUp.Domain.Enums;
using System;

namespace FocusUp.Application.Strategies
{
    public class TasksCompletedSingleDayBadgeRule : IBadgeRule
    {
        private readonly BadgeRuleType _ruleType = BadgeRuleType.tasks_completed_single_day;

        public TasksCompletedSingleDayBadgeRule() { }

        public bool IsUnlocked(BadgeContext context, Badge badge)
        {
            int taskToday = context.Tasks.Count(t => {
                if (t.CompletedAt == null) return false;
                return t.CompletedAt.Value.Date == DateTime.Now.Date;
            });
            return taskToday >= badge.RuleValue;
        }

        public BadgeRuleType GetRuleType() => _ruleType;
    }
}