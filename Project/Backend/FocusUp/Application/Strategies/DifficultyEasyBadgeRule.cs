using FocusUp.Application.Strategies.Interfaces;
using FocusUp.Domain.Enums;
using System;

namespace FocusUp.Application.Strategies
{
    public class DifficultyEasyBadgeRule : IBadgeRule
    {
        private readonly BadgeRuleType _ruleType = BadgeRuleType.difficulty_achived_easy;

        public DifficultyEasyBadgeRule()
        {
        }

        public bool IsUnlocked(BadgeContext context, Badge badge)
        {
            return context.Tasks.Count(t => t.Difficulty == TaskDifficultyType.Easy) > badge.RuleValue;
        }

        public BadgeRuleType GetRuleType() => _ruleType;
    }
}