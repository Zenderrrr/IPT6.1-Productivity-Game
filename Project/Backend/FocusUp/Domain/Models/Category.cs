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

    public void Rename(string newName) => Name = newName;

    public void ChangeColor(string newColor) => Color = newColor;

    public override bool ValidateData()
    {
        if(int.IsNegative(UserId))
            return false;

        if(string.IsNullOrWhiteSpace(Name))
            return false;

        if(string.IsNullOrWhiteSpace(Color))
            return false;

        return Color.Length == 7 &&
            Color[0] == '#';
    }
}