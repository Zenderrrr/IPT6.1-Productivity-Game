using FocusUp.Domain.Enums;
using System;

public class TaskLog : BaseModel
{
    public int UserId { get; }
    public int TaskId { get; }
    public RewardReason Action { get; }
    public int XpAwarded { get; }

    public TaskLog()
    {
    }

    public TaskLog(int userId, int taskId, RewardReason action, int xpAwarded)
    {
        UserId = userId;
        TaskId = taskId;
        Action = action;
        XpAwarded = xpAwarded;
    }

    public override bool ValidateData()
    {
        return
            !int.IsNegative(UserId) &&
            !int.IsNegative(TaskId) &&
            !int.IsNegative(XpAwarded);
    }
}