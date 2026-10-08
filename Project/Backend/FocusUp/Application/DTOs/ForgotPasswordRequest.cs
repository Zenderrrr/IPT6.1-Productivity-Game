using System;

namespace FocusUp.Application.DTOs
{
    public class ForgotPasswordRequest
    {
        public required string Email { get; set; } = string.Empty;
    }
}