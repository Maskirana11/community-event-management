using CommunityEvents.Models;

namespace CommunityEvents.Interfaces;

public interface IEventRepository : IGenericRepository<Event>
{
    // Specific methods for Event that might require complex queries
    Task<IEnumerable<Event>> GetUpcomingEventsAsync();
    Task<Event?> GetEventWithDetailsAsync(int id);
    Task<bool> HasAvailableCapacityAsync(int eventId);
}
