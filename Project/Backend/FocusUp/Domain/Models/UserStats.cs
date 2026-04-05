using System;

public class UserStats : BaseModel
{
    public int UserId { get; private set; }
    public int TotalXp { get; private set; } = 0;
    public int TasksDone { get; private set; } = 0;
    public int TasksOpen { get; private set; } = 0;
    public int TotalTimeMin { get; private set; } = 0;
    public int StreakCount { get; private set; } = 0;
    public int BestStreak {  get; private set; } = 0;
    public DateTime? StreakLastDate;
    public DateTime? LastActiveAt;

    public UserStats()
    {
    }

    public UserStats(int userId)
    {
        UserId = userId;
    }

    internal void SetId(int id)
    {
        Id = id;
    }

    public void ApplyTaskCompletion(int durationMin, DateTime today)
    {
        IncrementTasksDone();
        DecrementTasksOpen();
        AddDuration(durationMin);
        IncrementStreak(today);
        UpdateLastActive(today);
        UpdateDate();
    }

    public void AddXp(int amount)
    {
        TotalXp += amount;
    }

    private void IncrementTasksDone()
    {
        TasksDone++;
    }

    private void DecrementTasksOpen()
    {
        if(TasksOpen > 0)
            TasksOpen--;
    }

    private void AddDuration(int durationMin)
    {
        TotalTimeMin += durationMin;
    }

    public void RemoveTaskCompletion(int durationMin, DateTime today)
    {
        DecrementTasksDone();
        IncrementTasksOpen();
        RemoveDuration(durationMin);
        UpdateLastActive(today);
        UpdateDate();
    }

    private void RemoveXp(int amount)
    {
        if(TotalXp > amount)
            TotalXp -= amount;
    }

    private void DecrementTasksDone()
    {
        if(TasksDone > 0)
            TasksDone--;
    }


    private void IncrementTasksOpen()
    {
        TasksOpen++;
    }

    private void RemoveDuration(int durationMin)
    {
        if(TotalTimeMin > durationMin)
            TotalTimeMin -= durationMin;
    }

    public bool ShouldResetStreak(DateTime today, int gapDays)
    {
        if (StreakLastDate == null)
            return false;

        if (StreakLastDate == today)
            return false;

        if (StreakLastDate >= today.AddDays(-gapDays))
            return false;

        return true;
    }

    public void IncrementStreak(DateTime today)
    {
        StreakCount++;
        StreakLastDate = today;
    }

    public void ResetStreak()
    {
        StreakCount = 1;
    }

    public void UpdateLastActive(DateTime timestamp)
    {
        LastActiveAt = timestamp;
    }

    public override bool ValidateData()
    {
        return
            !int.IsNegative(UserId) &&
            !int.IsNegative(TotalXp) &&
            !int.IsNegative(TasksDone) &&
            !int.IsNegative(TasksOpen) &&
            !int.IsNegative(TotalTimeMin) &&
            !int.IsNegative(StreakCount) &&
            !int.IsNegative(BestStreak);
    }

    public override void UpdateDate()
    {
        UpdatedAt = DateTime.Now;
    }
}