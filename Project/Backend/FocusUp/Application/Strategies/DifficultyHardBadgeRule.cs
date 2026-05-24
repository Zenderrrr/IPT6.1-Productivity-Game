using FocusUp.Application.Strategies.Interfaces;
using FocusUp.Domain.Enums;
using System;

namespace FocusUp.Application.Strategies
{
    public class DifficultyHardBadgeRule : IBadgeRule
    {
        private readonly BadgeRuleType _ruleType = BadgeRuleType.difficulty_achived_hard;

        public DifficultyHardBadgeRule()
        {
        }

        public bool IsUnlocked(BadgeContext context, Badge badge)
        {
            return context.Tasks.Count(t => t.Difficulty == TaskDifficultyType.Hard) > badge.RuleValue;
        }

        public BadgeRuleType GetRuleType() => _ruleType;
    }
}