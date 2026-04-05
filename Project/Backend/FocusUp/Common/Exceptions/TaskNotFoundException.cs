using System;

namespace FocusUp.Common.Exceptions
{
    public class TaskNotFoundException : Exception
    {
        public TaskNotFoundException(int taskId) : base($"Task with ID {taskId} was not found.")
        {
        }
    }
}