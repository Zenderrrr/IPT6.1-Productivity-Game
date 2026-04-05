using System;
using static FocusUp.Domain.Enums.TaskStatus;
public class Task : BaseModel
{
    public int UserId { get; }
    public int? CategoryId { get; private set; }
    public string Title { get; private set; }
    public string Description { get; private set; }
    public int Difficulty { get; private set; }
    public int DurationMin { get; private set; }
    public DateTime? DueDate { get; private set; }
    public FocusUp.Domain.Enums.TaskStatus Status { get; private set; }
    public DateTime? CompletedAt { get; private set; }

    public Task()
    {
        Status = Open;
    }

    public Task(int userId, string title, string desciption, int difficulty, int? categoryId = null, DateTime? dueDate = null)
    {
        UserId = userId;
        Title = title;
        Description = desciption;
        Difficulty = difficulty;
        CategoryId = categoryId;
        DueDate = dueDate;

        Status = Open;
    }

    internal void SetId(int id)
    {
        Id = id;
    }

    public void MarkAsCompleted()
    {
        Status = Done;
        CompletedAt = DateTime.Now;
    }

    public void Reopen()
    {
        Status = Open;
        CompletedAt = null;
    }

    public void UpdateDetails(string title, string description, int difficulty, int durationMin, int? categoryId, DateTime? dueDate)
    {
        Title = title;
        Description = description;
        Difficulty = difficulty;
        DurationMin = durationMin;
        CategoryId = categoryId;
        DueDate = dueDate;
    }

    public bool IsCompleted()
    {
        return Status == Done;
    }

    public override bool ValidateData()
    {
        return
            !int.IsNegative(UserId) &&
            !string.IsNullOrWhiteSpace(Title) &&
            !string.IsNullOrWhiteSpace(Description) &&
            !int.IsNegative(Difficulty) &&
            !int.IsNegative(DurationMin);
    }

    public override void UpdateDate()
    {
        UpdatedAt = DateTime.Now;
    }
}