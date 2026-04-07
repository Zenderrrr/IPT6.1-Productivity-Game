using System;

namespace FocusUp.Common.Exceptions
{
    public class UnauthorizedTaskAccessException : Exception
    {
        public UnauthorizedTaskAccessException(int taskId, int userId) : base($"User with the ID {userId} has no permission to access Task ID {taskId}")
        {
        }
    }
}