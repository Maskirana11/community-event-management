using System.ComponentModel.DataAnnotations;

namespace CommunityEvents.Models;

public class Activity : BaseEntity
{
    [Required, MaxLength(200)]
    [Display(Name = "Activity Name")]
    public string Name { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string? Description { get; set; }

    [Required, MaxLength(100)]
    [Display(Name = "Activity Type")]
    public string Type { get; set; } = "Workshop";

    [Display(Name = "Duration (minutes)")]
    public int? DurationMinutes { get; set; }

    // Navigation properties
    public ICollection<EventActivity> EventActivities { get; set; } = new List<EventActivity>();
}
