using CommunityEvents.Models;

namespace CommunityEvents.ViewModels;

public class DashboardViewModel
{
    public int TotalEvents { get; set; }
    public int UpcomingEvents { get; set; }
    public int TotalParticipants { get; set; }
    public int TotalVenues { get; set; }
    public int TotalActivities { get; set; }
    public int TotalRegistrations { get; set; }
    public int PendingRegistrations { get; set; }
    public List<Event> RecentEvents { get; set; } = new();
    public List<Registration> RecentRegistrations { get; set; } = new();
}
