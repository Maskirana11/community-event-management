using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CommunityEvents.Data;
using CommunityEvents.Models;
using CommunityEvents.ViewModels;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace CommunityEvents.Controllers;

public class RegistrationsController : Controller
{
    private readonly ApplicationDbContext _context;
    public RegistrationsController(ApplicationDbContext context) { _context = context; }

    // GET: All registrations (admin view)
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Index()
    {
        var registrations = await _context.Registrations
            .Include(r => r.Participant)
            .Include(r => r.Event)
            .OrderByDescending(r => r.RegisteredAt)
            .ToListAsync();
        return View(registrations);
    }

    // GET: Register for event
    [Authorize]
    public async Task<IActionResult> Register(int eventId)
    {
        var ev = await _context.Events.FindAsync(eventId);
        if (ev == null) return NotFound();

        var vm = new RegisterViewModel
        {
            EventId = eventId,
            EventName = ev.Name
        };
        return View(vm);
    }

    // POST: Register for event
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize]
    public async Task<IActionResult> Register(RegisterViewModel vm)
    {
        var ev = await _context.Events
            .Include(e => e.Registrations)
            .FirstOrDefaultAsync(e => e.Id == vm.EventId);
        if (ev == null) return NotFound();

        // Capacity check
        if (ev.MaxCapacity.HasValue && ev.Registrations.Count >= ev.MaxCapacity.Value)
        {
            TempData["Error"] = "Sorry, this event is at full capacity.";
            return RedirectToAction("Details", "Events", new { id = vm.EventId });
        }

        var email = User.FindFirstValue(ClaimTypes.Email);
        var name = User.Identity?.Name ?? "Unknown";

        if (string.IsNullOrWhiteSpace(email))
        {
            TempData["Error"] = "Could not identify your email.";
            return RedirectToAction("Index", "Home");
        }

        var participant = await _context.Participants.FirstOrDefaultAsync(p => p.Email == email);
        if (participant == null)
        {
            // Auto-create participant for the logged-in user
            participant = new Participant
            {
                Name = name,
                Email = email
            };
            _context.Participants.Add(participant);
            await _context.SaveChangesAsync();
        }

        // Check if already registered
        var existing = await _context.Registrations
            .FirstOrDefaultAsync(r => r.ParticipantId == participant.Id && r.EventId == vm.EventId);

        if (existing != null)
        {
            TempData["Warning"] = "You are already registered for this event.";
            return RedirectToAction("Details", "Events", new { id = vm.EventId });
        }

        var registration = new Registration
        {
            ParticipantId = participant.Id,
            EventId = vm.EventId,
            Notes = vm.Notes,
            Status = RegistrationStatus.Confirmed,
            RegisteredAt = DateTime.UtcNow
        };

        _context.Registrations.Add(registration);
        await _context.SaveChangesAsync();

        TempData["Success"] = $"Successfully registered for '{ev.Name}'!";
        return RedirectToAction("MyRegistrations");
    }

    // GET: My Registrations
    [Authorize]
    public async Task<IActionResult> MyRegistrations()
    {
        var email = User.FindFirstValue(ClaimTypes.Email);
        if (string.IsNullOrWhiteSpace(email))
            return View(new List<Registration>());

        var participant = await _context.Participants.FirstOrDefaultAsync(p => p.Email == email);
        if (participant == null)
        {
            return View(new List<Registration>());
        }

        var registrations = await _context.Registrations
            .Where(r => r.ParticipantId == participant.Id)
            .Include(r => r.Event).ThenInclude(e => e.EventVenues).ThenInclude(ev => ev.Venue)
            .Include(r => r.Event).ThenInclude(e => e.EventActivities).ThenInclude(ea => ea.Activity)
            .OrderByDescending(r => r.Event.Date)
            .ToListAsync();

        ViewBag.ParticipantName = participant.Name;
        ViewBag.Email = email;
        return View(registrations);
    }

    // POST: Cancel registration
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize]
    public async Task<IActionResult> Cancel(int id)
    {
        var email = User.FindFirstValue(ClaimTypes.Email);
        var participant = await _context.Participants.FirstOrDefaultAsync(p => p.Email == email);

        if (participant == null) return NotFound();

        var reg = await _context.Registrations.FirstOrDefaultAsync(r => r.Id == id && r.ParticipantId == participant.Id);
        
        if (reg != null)
        {
            reg.Status = RegistrationStatus.Cancelled;
            await _context.SaveChangesAsync();
            TempData["Success"] = "Registration cancelled.";
        }
        return RedirectToAction(nameof(MyRegistrations));
    }

    // POST: Update status (admin)
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateStatus(int id, RegistrationStatus status)
    {
        var reg = await _context.Registrations.FindAsync(id);
        if (reg != null)
        {
            reg.Status = status;
            await _context.SaveChangesAsync();
            TempData["Success"] = "Registration status updated.";
        }
        return RedirectToAction(nameof(Index));
    }

    // POST: Delete registration (admin)
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var reg = await _context.Registrations.FindAsync(id);
        if (reg != null)
        {
            _context.Registrations.Remove(reg);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Registration deleted.";
        }
        return RedirectToAction(nameof(Index));
    }
}
