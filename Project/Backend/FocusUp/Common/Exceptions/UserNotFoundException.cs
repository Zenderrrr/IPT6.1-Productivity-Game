using FocusUp.Domain.Enums;
using System;

public class UserNotFoundException : Exception
{
    public UserNotFoundException(int userId) : base($"User from User ID {userId} was not found.")
    {
    }
}