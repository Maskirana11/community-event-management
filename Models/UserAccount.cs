using System.ComponentModel.DataAnnotations;

namespace CommunityEvents.Models;

public class UserAccount : BaseEntity
{
    [Required, EmailAddress, MaxLength(200)]
    public string Email { get; set; } = string.Empty;

    [Required, MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    public string PasswordHash { get; set; } = string.Empty;

    public int? ParticipantId { get; set; }
    public Participant? Participant { get; set; }
}