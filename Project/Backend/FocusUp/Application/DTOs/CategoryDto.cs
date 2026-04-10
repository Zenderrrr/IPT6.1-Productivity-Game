using System;

namespace FocusUp.Application.DTOs
{
    public class CategoryDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}