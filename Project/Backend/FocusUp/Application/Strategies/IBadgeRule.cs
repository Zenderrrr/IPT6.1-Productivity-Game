using System;

public interface IBadgeRule
{
    bool IsUnlocked(UserStats stats, Badge badge);
    string GetRuleType();
}