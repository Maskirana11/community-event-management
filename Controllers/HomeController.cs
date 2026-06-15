using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CommunityEvents.Data;
using CommunityEvents.ViewModels;

namespace CommunityEvents.Controllers;

public class HomeController : Controller
{
    private readonly ApplicationDbContext _context;

    public HomeController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var today = DateTime.Today;
        var vm = new DashboardViewModel
        {
            TotalEvents = await _context.Events.CountAsync(),
            UpcomingEvents = await _context.Events.CountAsync(e => e.Date >= today && e.IsActive),
            TotalParticipants = await _context.Participants.CountAsync(),
            TotalVenues = await _context.Venues.CountAsync(),
            TotalActivities = await _context.Activities.CountAsync(),
            TotalRegistrations = await _context.Registrations.CountAsync(),
            PendingRegistrations = await _context.Registrations.CountAsync(r => r.Status == Models.RegistrationStatus.Pending),
            RecentEvents = await _context.Events
                .Where(e => e.Date >= today && e.IsActive)
                .OrderBy(e => e.Date)
                .Include(e => e.EventVenues).ThenInclude(ev => ev.Venue)
                .Include(e => e.Registrations)
                .Take(6)
                .ToListAsync(),
            RecentRegistrations = await _context.Registrations
                .Include(r => r.Participant)
                .Include(r => r.Event)
                .OrderByDescending(r => r.RegisteredAt)
                .Take(5)
                .ToListAsync()
        };
        return View(vm);
    }

    public IActionResult Privacy() => View();

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() => View();
}
