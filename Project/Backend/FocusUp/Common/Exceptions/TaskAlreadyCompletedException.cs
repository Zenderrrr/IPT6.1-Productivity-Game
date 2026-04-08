using System;

namespace FocusUp.Common.Exceptions
{
    public class TaskAlreadyCompletedException : Exception
    {
        public TaskAlreadyCompletedException(int taskId) : base($"Task with ID {taskId} is already completed.")
        {
        }
    }
}