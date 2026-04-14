using FocusUp.Domain.Enums;
using System;

namespace FocusUp.Application.DTOs
{
    public class TaskDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int? CategoryId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public TaskDifficultyType Difficulty { get; set; }
        public int DurationMin { get; set; }
        public DateTime? DueDate { get; set; }
        public Domain.Enums.TaskStatus Status { get; set; }
        public DateTime? CompletedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int Xp { get; set; }

    }
}