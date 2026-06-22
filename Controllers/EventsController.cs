using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CommunityEvents.Data;
using CommunityEvents.Models;
using CommunityEvents.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace CommunityEvents.Controllers;

public class EventsController : Controller
{
    private readonly ApplicationDbContext _context;

    public EventsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: Events
    public async Task<IActionResult> Index(EventFilterViewModel filter)
    {
        var query = _context.Events
            .Include(e => e.EventVenues).ThenInclude(ev => ev.Venue)
            .Include(e => e.EventActivities).ThenInclude(ea => ea.Activity)
            .Include(e => e.Registrations)
            .Where(e => e.IsActive)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
            query = query.Where(e => e.Name.Contains(filter.SearchTerm) || e.Description.Contains(filter.SearchTerm));

        if (filter.DateFrom.HasValue)
            query = query.Where(e => e.Date >= filter.DateFrom.Value);

        if (filter.DateTo.HasValue)
            query = query.Where(e => e.Date <= filter.DateTo.Value);

        if (filter.VenueId.HasValue)
            query = query.Where(e => e.EventVenues.Any(ev => ev.VenueId == filter.VenueId.Value));

        if (!string.IsNullOrWhiteSpace(filter.ActivityType))
            query = query.Where(e => e.EventActivities.Any(ea => ea.Activity.Type == filter.ActivityType));

        if (!string.IsNullOrWhiteSpace(filter.EventType))
            query = query.Where(e => e.EventType == filter.EventType);

        var events = await query.OrderBy(e => e.Date).ToListAsync();

        ViewBag.Venues = new SelectList(await _context.Venues.ToListAsync(), "Id", "Name", filter.VenueId);
        ViewBag.ActivityTypes = new SelectList(await _context.Activities.Select(a => a.Type).Distinct().ToListAsync());
        ViewBag.EventTypes = new SelectList(await _context.Events.Select(e => e.EventType).Distinct().ToListAsync());
        ViewBag.Filter = filter;

        return View(events);
    }

    // GET: Events/Details/5
    public async Task<IActionResult> Details(int id)
    {
        var ev = await _context.Events
            .Include(e => e.EventVenues).ThenInclude(ev => ev.Venue)
            .Include(e => e.EventActivities).ThenInclude(ea => ea.Activity)
            .Include(e => e.Registrations).ThenInclude(r => r.Participant)
            .FirstOrDefaultAsync(e => e.Id == id);

        if (ev == null) return NotFound();

        var vm = new EventDetailsViewModel
        {
            Event = ev,
            Registrations = ev.Registrations.ToList()
        };

        return View(vm);
    }

    // GET: Events/Create
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create()
    {
        var vm = new EventCreateEditViewModel
        {
            AllVenues = await _context.Venues.ToListAsync(),
            AllActivities = await _context.Activities.ToListAsync()
        };
        return View(vm);
    }

    // POST: Events/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create(EventCreateEditViewModel vm)
    {
        ModelState.Remove("Event.EventVenues");
        ModelState.Remove("Event.EventActivities");
        ModelState.Remove("Event.Registrations");

        if (ModelState.IsValid)
        {
            _context.Events.Add(vm.Event);
            await _context.SaveChangesAsync();

            foreach (var venueId in vm.SelectedVenueIds)
                _context.EventVenues.Add(new EventVenue { EventId = vm.Event.Id, VenueId = venueId });

            foreach (var actId in vm.SelectedActivityIds)
                _context.EventActivities.Add(new EventActivity { EventId = vm.Event.Id, ActivityId = actId });

            await _context.SaveChangesAsync();
            TempData["Success"] = $"Event '{vm.Event.Name}' created successfully!";
            return RedirectToAction(nameof(Index));
        }

        vm.AllVenues = await _context.Venues.ToListAsync();
        vm.AllActivities = await _context.Activities.ToListAsync();
        return View(vm);
    }

    // GET: Events/Edit/5
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Edit(int id)
    {
        var ev = await _context.Events
            .Include(e => e.EventVenues)
            .Include(e => e.EventActivities)
            .FirstOrDefaultAsync(e => e.Id == id);

        if (ev == null) return NotFound();

        var vm = new EventCreateEditViewModel
        {
            Event = ev,
            SelectedVenueIds = ev.EventVenues.Select(ev2 => ev2.VenueId).ToList(),
            SelectedActivityIds = ev.EventActivities.Select(ea => ea.ActivityId).ToList(),
            AllVenues = await _context.Venues.ToListAsync(),
            AllActivities = await _context.Activities.ToListAsync()
        };
        return View(vm);
    }

    // POST: Events/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Edit(int id, EventCreateEditViewModel vm)
    {
        if (id != vm.Event.Id) return NotFound();

        ModelState.Remove("Event.EventVenues");
        ModelState.Remove("Event.EventActivities");
        ModelState.Remove("Event.Registrations");

        if (ModelState.IsValid)
        {
            _context.Update(vm.Event);

            // Update venues
            var existingVenues = await _context.EventVenues.Where(ev => ev.EventId == id).ToListAsync();
            _context.EventVenues.RemoveRange(existingVenues);
            foreach (var venueId in vm.SelectedVenueIds)
                _context.EventVenues.Add(new EventVenue { EventId = id, VenueId = venueId });

            // Update activities
            var existingActivities = await _context.EventActivities.Where(ea => ea.EventId == id).ToListAsync();
            _context.EventActivities.RemoveRange(existingActivities);
            foreach (var actId in vm.SelectedActivityIds)
                _context.EventActivities.Add(new EventActivity { EventId = id, ActivityId = actId });

            await _context.SaveChangesAsync();
            TempData["Success"] = $"Event '{vm.Event.Name}' updated successfully!";
            return RedirectToAction(nameof(Index));
        }

        vm.AllVenues = await _context.Venues.ToListAsync();
        vm.AllActivities = await _context.Activities.ToListAsync();
        return View(vm);
    }

    // GET: Events/Delete/5
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var ev = await _context.Events
            .Include(e => e.EventVenues).ThenInclude(ev => ev.Venue)
            .FirstOrDefaultAsync(e => e.Id == id);
        if (ev == null) return NotFound();
        return View(ev);
    }

    // POST: Events/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var ev = await _context.Events.FindAsync(id);
        if (ev != null)
        {
            _context.Events.Remove(ev);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Event deleted successfully!";
        }
        return RedirectToAction(nameof(Index));
    }
}
