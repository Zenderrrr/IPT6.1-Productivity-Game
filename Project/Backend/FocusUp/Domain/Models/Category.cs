using System;

public class Category : BaseModel
{
    public int UserId { get; }
    public string Name { get; private set; }
    public string Color { get; private set; }

    public Category() { }

    public Category(int userId, string name, string color)
    {
        UserId = userId;
        Name = name;
        Color = color;
    }

    internal void SetId(int id)
    {
        Id = id;
    }

    public void Rename(string newName)
    {
        Name = newName;
    }

    public void ChangeColor(string newColor)
    {
        Color = newColor;
    }

    public override bool ValidateData()
    {
        return
            !int.IsNegative(UserId) &&
            !string.IsNullOrWhiteSpace(Name) &&
            !string.IsNullOrWhiteSpace(Color);
    }
}