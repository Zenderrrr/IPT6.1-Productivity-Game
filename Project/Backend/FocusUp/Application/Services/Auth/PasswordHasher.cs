using System;

namespace FocusUp.Application.Services.Auth
{
    public class PasswordHasher
    {
        public string PasswordHashing(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public bool PasswordVerify(string password, string hash)
        {
            return BCrypt.Net.BCrypt.Verify(password, hash);
        }
    }
}