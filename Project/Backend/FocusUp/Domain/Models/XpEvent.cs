using FocusUp.Domain.Enums;
using System;

public class XpEvent : BaseModel
{
    public int UserId { get; }
    public int? TaskId { get; }
    public int Amount { get; }
    public RewardReason Reason { get; }

    public XpEvent()
    {
    }

    public XpEvent(int userId, int amount, RewardReason reason, int? taskId = null)
    {
        UserId = userId;
        TaskId = taskId;
        Amount = amount;
        Reason = reason;
    }

    internal void SetId(int id)
    {
        Id = id;
    }

    public override bool ValidateData()
    {
        return
            !int.IsNegative(UserId) &&
            !int.IsNegative(Amount);
    }

}