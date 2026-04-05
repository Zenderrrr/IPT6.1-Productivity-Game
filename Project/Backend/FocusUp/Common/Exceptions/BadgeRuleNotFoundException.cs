using FocusUp.Domain.Enums;
using System;

public class BadgeRuleNotFoundException : Exception
{
    public BadgeRuleNotFoundException(BadgeRuleType badgeRuleType, int badgeId) : base($"Badgerule {badgeRuleType} for Badge ID {badgeId} was not found.")
    {
    }
}