using FocusUp.Application.Strategies;
using FocusUp.Application.Strategies.Interfaces;
using FocusUp.Domain.Enums;
using System;

public class ComebackAfterBreakBadgeRule : IBadgeRule
{
    private readonly BadgeRuleType _ruleType = BadgeRuleType.comeback_after_break;

    public ComebackAfterBreakBadgeRule()
    {
    }

    public bool IsUnlocked(BadgeContext context, Badge badge)
    {
        if(context.Stats.LastActiveAt == null) return false;

        int gap = new DateTime().Day - context.Stats.LastActiveAt.Value.Day;
        return gap >= badge.RuleValue;
    }

    public BadgeRuleType GetRuleType() => _ruleType;
}