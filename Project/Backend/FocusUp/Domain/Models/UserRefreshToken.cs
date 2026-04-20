using System;

namespace FocusUp.Domain.Models
{
    public class UserRefreshToken : BaseModel
    {
        public int UserId { get; private set; }
        public string TokenHash { get; private set; }
        public DateTime ExpiresAt { get; private set; }
        public DateTime? RevokedAt { get; private set; }

        public UserRefreshToken(int id, int userId, string tokenHash, DateTime createdAt, DateTime expiresAt, DateTime? revokedAt = null)
        {
            Id = id;
            UserId = userId;
            TokenHash = tokenHash;
            ExpiresAt = expiresAt;
            RevokedAt = revokedAt;
            CreatedAt = createdAt;
        }

        public UserRefreshToken(int userId, string tokenHash, DateTime expiresAt)
        {
            UserId = userId;
            TokenHash = tokenHash;
            ExpiresAt = expiresAt;
        }

        public void SetRevokedAt(DateTime date) => RevokedAt = date;

        public bool IsRevoked() => RevokedAt != null;

        public bool IsExpired() => ExpiresAt <= DateTime.UtcNow;

        public bool IsActive() => !IsRevoked() && !IsExpired();
    }
}