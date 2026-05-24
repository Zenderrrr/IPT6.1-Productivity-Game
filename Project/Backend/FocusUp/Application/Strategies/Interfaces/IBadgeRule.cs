using FocusUp.Domain.Enums;
using System;

namespace FocusUp.Application.Strategies.Interfaces
{
    public interface IBadgeRule
    {
        bool IsUnlocked(BadgeContext context, Badge badge);
        BadgeRuleType GetRuleType();
    }
}