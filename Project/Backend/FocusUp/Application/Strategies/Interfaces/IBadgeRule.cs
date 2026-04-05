using FocusUp.Domain.Enums;
using System;

namespace FocusUp.Application.Strategies.Interfaces
{
    public interface IBadgeRule
    {
        bool IsUnlocked(UserStats stats, Badge badge);
        BadgeRuleType GetRuleType();
    }
}