using System;

namespace FocusUp.Application.DTOs
{
    public class StatsDto
    {
        public int TasksDone { get; set; } = 0;
        public int TasksOpen { get; set; } = 0;
        public int TotalTimeMin { get; set; } = 0;
        public int TotalXp { get; set; } = 0;
        public int StreakCount { get; set; } = 0;
        public int BestStreak { get; set; } = 0;
    }
}