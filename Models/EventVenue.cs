namespace CommunityEvents.Models;

public class EventVenue
{
    public int EventId { get; set; }
    public int VenueId { get; set; }

    // Navigation properties
    public Event Event { get; set; } = null!;
    public Venue Venue { get; set; } = null!;
}
