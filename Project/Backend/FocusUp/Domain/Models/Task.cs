using System

public class Task
{
    public int Id;
    public int UserId;
    public int? CategoryId;
    public string Title;
    public string Description;
    public int Difficulty;
    public int DurationMin;
    public DateTime? DueDate;
    public string Status;
    public DateTime? CompletedAt;
    public DateTime CreatedAt;
    public DateTime UpdatedAt;

    public Task()
    {
    }

    public Task(int userId, string title, string desciption, int difficulty, int? categoryId = null, DateTime? dueDate = null)
    {
        UserId = userId;
        Title = title;
        Description = desciption;
        Difficulty = difficulty;
        CategoryId = categoryId;
        DueDate = dueDate;
    }

    public void MarkAsCompleted()
    {
        throw new NotImplementedException();
    }

    public void Reopen()
    {
        throw new NotImplementedException(); 
    }

    public void UpdateDetails(string title, string description, int difficulty, int durationMin, int? categoryId, DateTime? dueDate)
    {
        throw new NotImplementedException();
    }

    public bool IsCompleted()
    {
        throw new NotImplementedException(); 
    }

    public bool ValidateData()
    {
        throw new NotImplementedException();
    }

    public void UpdateData()
    {
        throw new NotImplementedException(); 
    }
}