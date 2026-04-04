using System;

public class XpEvent : BaseModel
{
    public int UserId { get; }
    public int? TaskId { get; }
    public int Amount { get; }
    public string Reason { get; }

    public XpEvent()
    {
    }

    public XpEvent(int userId, int amount, string reason, int? taskId = null)
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
            !int.IsNegative(Amount) &&
            !string.IsNullOrWhiteSpace(Reason);
    }

}