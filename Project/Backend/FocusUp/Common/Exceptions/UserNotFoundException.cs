using FocusUp.Domain.Enums;
using System;

namespace FocusUp.Common.Exceptions
{
    public class UserNotFoundException : Exception
    {
        public UserNotFoundException(int userId) : base($"User from User ID {userId} was not found.")
        {
        }
    }
}