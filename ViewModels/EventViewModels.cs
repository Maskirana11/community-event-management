using CommunityEvents.Models;

namespace CommunityEvents.ViewModels;

public class EventCreateEditViewModel
{
    public Event Event { get; set; } = new();
    public List<int> SelectedVenueIds { get; set; } = new();
    public List<int> SelectedActivityIds { get; set; } = new();
    public List<Venue> AllVenues { get; set; } = new();
    public List<Activity> AllActivities { get; set; } = new();
}

public class EventDetailsViewModel
{
    public Event Event { get; set; } = null!;
    public List<Registration> Registrations { get; set; } = new();
    public bool IsRegistered { get; set; }
    public int ParticipantId { get; set; }
}

public class RegisterViewModel
{
    public int EventId { get; set; }
    public string EventName { get; set; } = string.Empty;
    public Participant? Participant { get; set; }
    public string? Notes { get; set; }
    public string? Email { get; set; }
}
