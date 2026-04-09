using System;

namespace FocusUp.Application.DTOs
{
    public class ProductivityDto
    {
        public DateTime Date {  get; set; } = DateTime.Now;
        public int CompletedTasks { get; set; } = 0;
        public int XPGained { get; set; } = 0;
        public int TimeSpent { get; set; } = 0;
    }
}