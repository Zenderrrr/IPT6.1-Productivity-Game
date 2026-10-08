using System;

namespace FocusUp.Domain.Models
{
    public class PasswordResetToken : BaseModel
    {
        public int UserId { get; set; }
        public string TokenHash { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
        public DateTime? UsedAt { get; set; }

        public PasswordResetToken()
        {
        }

        public PasswordResetToken(int userId, string tokenHash, DateTime expiresAt, DateTime? usedAt)
        {
            UserId = userId;
            TokenHash = tokenHash;
            ExpiresAt = expiresAt;
            UsedAt = usedAt;
        }
    }
}