using FocusUp.Application.Strategies.Interfaces;
using FocusUp.Domain.Enums;
using System;

public class CombackAfterBrackBadgeRule : IBadgeRule
{
    private readonly BadgeRuleType _ruleType = BadgeRuleType.comeback_after_break;

    public CombackAfterBrackBadgeRule()
    {
    }

    public bool IsUnlocked(UserStats stats, Badge badge)
    {
        throw new NotImplementedException();
    }

    public BadgeRuleType GetRuleType() => _ruleType;
}