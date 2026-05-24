using FocusUp.Application.Strategies.Interfaces;
using FocusUp.Domain.Enums;
using System;

namespace FocusUp.Application.Strategies
{
    public class DifficultyHardcoreBadgeRule : IBadgeRule
    {
        private readonly BadgeRuleType _ruleType = BadgeRuleType.difficulty_achived_hardcore;

        public DifficultyHardcoreBadgeRule()
        {
        }

        public bool IsUnlocked(BadgeContext context, Badge badge)
        {
            var tasks = context.Tasks
                .Where(t => t.CompletedAt != null)
                .OrderBy(t => t.CompletedAt)
                .Take(badge.RuleValue)
                .ToList();

            if (tasks.Count < badge.RuleValue) return false;

            return tasks.All(t => t.Difficulty == TaskDifficultyType.Hard);
        }

        public BadgeRuleType GetRuleType() => _ruleType;
    }
}