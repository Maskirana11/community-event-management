using System.ComponentModel.DataAnnotations;

namespace CommunityEvents.Models;

public enum RegistrationStatus
{
    Pending,
    Confirmed,
    Cancelled,
    Attended
}

public class Registration : BaseEntity
{
    [Required]
    public int ParticipantId { get; set; }

    [Required]
    public int EventId { get; set; }

    [Display(Name = "Registered On")]
    public DateTime RegisteredAt { get; set; } = DateTime.UtcNow;

    public RegistrationStatus Status { get; set; } = RegistrationStatus.Pending;

    [MaxLength(500)]
    public string? Notes { get; set; }

    // Navigation properties
    public Participant Participant { get; set; } = null!;
    public Event Event { get; set; } = null!;
}
