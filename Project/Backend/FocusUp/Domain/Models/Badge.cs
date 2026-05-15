using FocusUp.Domain.Enums;
using System;
using System.Text.Json.Serialization;

public class Badge : BaseModel
{
    public string Name { get; private set; }
    public string Description { get; private set; }
    public BadgeRuleType RuleType { get; private set; }
    public int RuleValue { get; private set; }
    public BadgeRarity Rarity { get; private set; }
    public string PrimaryColor { get; private set; }
    public string SecondaryColor { get; private set; }

    public Badge()
    {
    }

    public Badge(string name, string description, BadgeRuleType ruleType, int ruleValue, BadgeRarity rarity, string primaryColor, string secondaryColor)
    {
        Name = name;
        Description = description;
        RuleType = ruleType;
        RuleValue = ruleValue;
        Rarity = rarity;
        PrimaryColor = primaryColor;
        SecondaryColor = secondaryColor;
    }

    public void Rename(string newName) => Name = newName;
    public void NewDescription(string newDescription) => Description = newDescription;
    public void UpdateRule(BadgeRuleType ruleType, int ruleValue)
    {
        RuleType = ruleType;
        RuleValue = ruleValue;
    }

    public override bool ValidateData()
    {
        return
            !string.IsNullOrWhiteSpace(Name) &&
            !string.IsNullOrWhiteSpace(Description) &&
            !int.IsNegative(RuleValue) &&
            !string.IsNullOrWhiteSpace(PrimaryColor) &&
            !string.IsNullOrWhiteSpace(SecondaryColor);
    }
}