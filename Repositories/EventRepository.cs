using CommunityEvents.Data;
using CommunityEvents.Interfaces;
using CommunityEvents.Models;
using Microsoft.EntityFrameworkCore;

namespace CommunityEvents.Repositories;

public class EventRepository : GenericRepository<Event>, IEventRepository
{
    public EventRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Event>> GetUpcomingEventsAsync()
    {
        return await _context.Events
            .Where(e => e.Date >= DateTime.Today && e.IsActive)
            .OrderBy(e => e.Date)
            .ThenBy(e => e.Time)
            .ToListAsync();
    }

    public async Task<Event?> GetEventWithDetailsAsync(int id)
    {
        return await _context.Events
            .Include(e => e.EventVenues)
                .ThenInclude(ev => ev.Venue)
            .Include(e => e.EventActivities)
                .ThenInclude(ea => ea.Activity)
            .Include(e => e.Registrations)
                .ThenInclude(r => r.Participant)
            .FirstOrDefaultAsync(m => m.Id == id);
    }

    public async Task<bool> HasAvailableCapacityAsync(int eventId)
    {
        var ev = await _context.Events
            .Include(e => e.Registrations)
            .FirstOrDefaultAsync(e => e.Id == eventId);

        if (ev == null) return false;
        
        if (!ev.MaxCapacity.HasValue) return true; // Unlimited capacity

        int activeRegistrations = ev.Registrations.Count(r => r.Status != RegistrationStatus.Cancelled);
        return activeRegistrations < ev.MaxCapacity.Value;
    }
}
