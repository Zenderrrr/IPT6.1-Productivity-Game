using System;

namespace FocusUp.Application.DTOs
{
    public class CreateTaskRequest
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Difficulty { get; set; } = string.Empty;
        public int DurationMin { get; set; } = 0;
        public int? CategoryId { get; set; } = null ;
        public DateTime? DueDate { get; set; } = null;
    }
}