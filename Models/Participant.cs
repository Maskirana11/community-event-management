using System.ComponentModel.DataAnnotations;

namespace CommunityEvents.Models;

public class Participant : BaseEntity
{
    [Required, MaxLength(100)]
    [Display(Name = "Full Name")]
    public string Name { get; set; } = string.Empty;

    [Required, EmailAddress, MaxLength(200)]
    public string Email { get; set; } = string.Empty;

    [Phone, MaxLength(20)]
    [Display(Name = "Phone Number")]
    public string? Phone { get; set; }

    [MaxLength(200)]
    public string? Organization { get; set; }

    public DateTime RegisteredAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public ICollection<Registration> Registrations { get; set; } = new List<Registration>();
}
