using System;

public class UserBadge : BaseModel
{
    public int UserId { get; }
    public int BadgeId { get; }
    public DateTime AwardedAt { get; }

    public UserBadge()
    {
    }

    public UserBadge(int userId, int badgeId, DateTime awardedAt)
    {
        UserId = userId;
        BadgeId = badgeId;
        AwardedAt = awardedAt;
    }

    internal void SetId(int id)
    {
        Id = id;
    }

    public override bool ValidateData()
    {
        return
            !int.IsNegative(UserId) &&
            !int.IsNegative(BadgeId);
    }
}