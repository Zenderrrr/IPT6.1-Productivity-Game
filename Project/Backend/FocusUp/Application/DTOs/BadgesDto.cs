using FocusUp.Domain.Enums;
using System;
using System.Text.Json.Serialization;

namespace FocusUp.Application.DTOs
{
    public class BadgesDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public BadgeRuleType RuleType { get; set; }
        public int RuleValue { get; set; } = 0;
        public DateTime CreatedAt { get; set; }
    }
}