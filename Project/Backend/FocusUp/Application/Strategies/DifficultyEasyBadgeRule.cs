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

        public bool IsUnlocked(UserStats stats, Badge badge)
        {
            throw new NotImplementedException();
        }

        public BadgeRuleType GetRuleType() => _ruleType;
    }
}