using System;

public class UserStats : BaseModel
{
    public int UserId { get; private set; }
    public int TotalXp { get; private set; }
    public int TasksDone { get; private set; }
    public int TasksOpen { get; private set; }
    public int TotalTimeMin { get; private set; }
    public int StreakCount { get; private set; }
    public int BestStreak {  get; private set; }
    public DateTime? StreakLastDate;
    public DateTime? LastActiveAt;

    public UserStats()
    {
    }

    public UserStats(int userId)
    {
        UserId = userId;

        TotalXp = 0;
        TasksDone = 0;
        TasksOpen = 0;
        TotalTimeMin = 0;
        StreakCount = 0;
        BestStreak = 0;
    }

    public UserStats(int userId, int totalXp, int tasksDone, int tasksOpen, int totalTimeMin, int streakCount, int bestStreak, DateTime? streakLastDate = null, DateTime? lastActiveAt = null)
    {
        UserId = userId;
        TotalXp = totalXp;
        TasksDone = tasksDone;
        TotalTimeMin = totalTimeMin;
        StreakCount = streakCount;
        BestStreak = bestStreak;
        StreakLastDate = streakLastDate;
        LastActiveAt = lastActiveAt;
        TasksOpen = tasksOpen;
    }

    public void ApplyTaskCompletion(int durationMin, DateTime today)
    {
        IncrementTasksDone();
        DecrementTasksOpen();
        AddDuration(durationMin);
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

    public void DecrementTasksOpen()
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


    public void IncrementTasksOpen() => TasksOpen++;

    private void RemoveDuration(int durationMin)
    {
        if(TotalTimeMin > durationMin)
            TotalTimeMin -= durationMin;
    }

    public bool ShouldResetStreak(DateTime today, int gapDays)
    {
        if (StreakLastDate == null)
            return false;

        if (StreakLastDate.Value.Date == today.Date)
            return false;

        if (StreakLastDate.Value.Date >= today.AddDays(-gapDays).Date)
            return false;

        return true;
    }

    public void IncrementStreak(DateTime today)
    {
        StreakCount++;
        StreakLastDate = today;
    }

    public void SetStreak(int currentStreak, DateTime completedAt)
    {
        StreakCount = currentStreak;
        StreakLastDate = completedAt;

        if(currentStreak > BestStreak)
            BestStreak = currentStreak;
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