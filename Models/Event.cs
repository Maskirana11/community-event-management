using System.ComponentModel.DataAnnotations;

namespace CommunityEvents.Models;

public class Event
{
    public int Id { get; set; }

    [Required, MaxLength(200)]
    [Display(Name = "Event Name")]
    public string Name { get; set; } = string.Empty;

    [Required]
    public string Description { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.Date)]
    [Display(Name = "Event Date")]
    public DateTime Date { get; set; }

    [Required]
    [DataType(DataType.Time)]
    [Display(Name = "Start Time")]
    public TimeSpan Time { get; set; }

    [MaxLength(100)]
    [Display(Name = "Event Type")]
    public string EventType { get; set; } = "General";

    public string? ImageUrl { get; set; }

    [Display(Name = "Max Capacity")]
    public int? MaxCapacity { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public ICollection<Registration> Registrations { get; set; } = new List<Registration>();
    public ICollection<EventVenue> EventVenues { get; set; } = new List<EventVenue>();
    public ICollection<EventActivity> EventActivities { get; set; } = new List<EventActivity>();
}
