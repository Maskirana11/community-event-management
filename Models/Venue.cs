using System.ComponentModel.DataAnnotations;

namespace CommunityEvents.Models;

public class Venue : BaseEntity
{
    [Required, MaxLength(200)]
    [Display(Name = "Venue Name")]
    public string Name { get; set; } = string.Empty;

    [Required, MaxLength(300)]
    public string Address { get; set; } = string.Empty;

    [Required, MaxLength(100)]
    public string City { get; set; } = string.Empty;

    [Range(1, 100000)]
    public int Capacity { get; set; }

    [MaxLength(500)]
    public string? Description { get; set; }

    [Phone]
    [Display(Name = "Contact Phone")]
    public string? ContactPhone { get; set; }

    [EmailAddress]
    [Display(Name = "Contact Email")]
    public string? ContactEmail { get; set; }

    // Navigation properties
    public ICollection<EventVenue> EventVenues { get; set; } = new List<EventVenue>();
}
