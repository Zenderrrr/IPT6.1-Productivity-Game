using System;

namespace FocusUp.Application.DTOs
{
    public class TaskCompleteDto
    {
        public int StreakCountBefore { get; set; }
        public int StreakCountAfter { get; set; }
        public bool IsStreakIncreased { get; set; }
        public Task Task { get; set; } = new();
        public int Xp { get; set; }
        public List<Badge> Badges { get; set; } = new();
    }
}