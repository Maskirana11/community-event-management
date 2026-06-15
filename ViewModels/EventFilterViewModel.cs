namespace CommunityEvents.ViewModels;

public class EventFilterViewModel
{
    public string? SearchTerm { get; set; }
    public DateTime? DateFrom { get; set; }
    public DateTime? DateTo { get; set; }
    public int? VenueId { get; set; }
    public string? ActivityType { get; set; }
    public string? EventType { get; set; }
}
