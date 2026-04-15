using FocusUp.Domain.Enums;
using System;
using static FocusUp.Domain.Enums.TaskStatus;
public class Task : BaseModel
{
    public int UserId { get; }
    public int? CategoryId { get; private set; }
    public string Title { get; private set; }
    public string Description { get; private set; }
    public TaskDifficultyType Difficulty { get; private set; }
    public int DurationMin { get; private set; }
    public DateTime? DueDate { get; private set; }
    public FocusUp.Domain.Enums.TaskStatus Status { get; private set; }
    public DateTime? CompletedAt { get; private set; }

    public Task() => Status = Open;
    public Task(int userId, string title, string desciption, TaskDifficultyType difficulty, int durationMin, FocusUp.Domain.Enums.TaskStatus status , int? categoryId = null, DateTime? dueDate = null)
    {
        UserId = userId;
        Title = title;
        Description = desciption;
        Difficulty = difficulty;
        CategoryId = categoryId;
        DueDate = dueDate;
        DurationMin = durationMin;
        Status = status;
    }

    public Task(int id, int userId, string title, string desciption, TaskDifficultyType difficulty, int durationMin, FocusUp.Domain.Enums.TaskStatus status, int? categoryId = null, DateTime? dueDate = null)
    {
        Id = id;
        UserId = userId;
        Title = title;
        Description = desciption;
        Difficulty = difficulty;
        CategoryId = categoryId;
        DueDate = dueDate;
        DurationMin = durationMin;
        Status = status;
    }

    public void SetCompletedAt(DateTime? dateTime) => CompletedAt = dateTime;

    public void MarkAsCompleted()
    {
        Status = Completed;
        CompletedAt = DateTime.Now;
    }

    public void Reopen()
    {
        Status = Open;
        CompletedAt = null;
    }

    public void UpdateDetails(string title, string description, TaskDifficultyType difficulty, int durationMin, int? categoryId, DateTime? dueDate)
    {
        Title = title;
        Description = description;
        Difficulty = difficulty;
        DurationMin = durationMin;
        CategoryId = categoryId;
        DueDate = dueDate;
    }

    public bool IsCompleted() => Status == Completed;

    public override bool ValidateData()
    {
        return
            !int.IsNegative(UserId) &&
            !string.IsNullOrWhiteSpace(Title) &&
            !int.IsNegative(DurationMin);
    }

    public override void UpdateDate() => UpdatedAt = DateTime.Now;
}