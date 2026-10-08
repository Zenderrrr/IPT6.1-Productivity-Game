using System;

public class ResetPasswordRequest
{
    public required string Token { get; set; } = string.Empty;
    public required string NewPassword { get; set; } = string.Empty;
}