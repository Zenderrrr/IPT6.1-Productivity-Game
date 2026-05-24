using System;

namespace FocusUp.Application.Strategies
{
    public class BadgeContext
    {
        public BadgeContext(UserStats stats, List<Task> tasks)
        {
            Stats = stats;
            Tasks = tasks;
        }

        public UserStats Stats { get; set; } = null!;
        public List<Task> Tasks { get; set; } = new();
    }
}