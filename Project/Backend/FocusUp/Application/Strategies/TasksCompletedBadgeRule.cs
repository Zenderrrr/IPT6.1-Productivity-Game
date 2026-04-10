using FocusUp.Application.Strategies.Interfaces;
using FocusUp.Domain.Enums;
using System;

namespace FocusUp.Application.Strategies
{
    public class TasksCompletedBadgeRule : IBadgeRule
    {
        private readonly BadgeRuleType _ruleType = BadgeRuleType.tasks_completed;

        public TasksCompletedBadgeRule() { }

        public bool IsUnlocked(UserStats stats, Badge badge) => stats.TasksDone >= badge.RuleValue;

        public BadgeRuleType GetRuleType() => _ruleType;
    }
}