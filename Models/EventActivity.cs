namespace CommunityEvents.Models;

public class EventActivity
{
    public int EventId { get; set; }
    public int ActivityId { get; set; }

    // Navigation properties
    public Event Event { get; set; } = null!;
    public Activity Activity { get; set; } = null!;
}
