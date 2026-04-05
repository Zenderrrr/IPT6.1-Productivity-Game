using System;

public class Badge : BaseModel
{
    public string Name { get; private set; }
    public string Description { get; private set; }
    public string RuleType { get; private set; }
    public int RuleValue { get; private set; }

    public Badge()
    {
    }

    public Badge(string name, string description, string ruleType, int ruleValue)
    {
        Name = name;
        Description = description;
        RuleType = ruleType;
        RuleValue = ruleValue;
    }

    internal void SetId(int id)
    {
        Id = id;
    }

    public void Rename(string newName)
    {
        Name = newName;
    }

    public void NewDescription(string newDescription)
    {
        Description = newDescription;
    }

    public void UpdateRule(string ruleType, int ruleValue)
    {
        RuleType = ruleType;
        RuleValue = ruleValue;
    }

    public override bool ValidateData()
    {
        return
            !string.IsNullOrWhiteSpace(Name) &&
            !string.IsNullOrWhiteSpace(Description) &&
            !string.IsNullOrWhiteSpace(RuleType) &&
            !int.IsNegative(RuleValue);
    }
}