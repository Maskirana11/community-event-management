using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CommunityEvents.Data;
using CommunityEvents.Models;

using Microsoft.AspNetCore.Authorization;

namespace CommunityEvents.Controllers;

[Authorize(Roles = "Admin")]
public class ParticipantsController : Controller
{
    private readonly ApplicationDbContext _context;
    public ParticipantsController(ApplicationDbContext context) { _context = context; }

    public async Task<IActionResult> Index(string? search)
    {
        var query = _context.Participants
            .Include(p => p.Registrations)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(p => p.Name.Contains(search) || p.Email.Contains(search));

        ViewBag.Search = search;
        return View(await query.OrderBy(p => p.Name).ToListAsync());
    }

    public async Task<IActionResult> Details(int id)
    {
        var participant = await _context.Participants
            .Include(p => p.Registrations).ThenInclude(r => r.Event)
            .FirstOrDefaultAsync(p => p.Id == id);
        if (participant == null) return NotFound();
        return View(participant);
    }

    public IActionResult Create() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Participant participant)
    {
        ModelState.Remove("Registrations");
        if (ModelState.IsValid)
        {
            if (await _context.Participants.AnyAsync(p => p.Email == participant.Email))
            {
                ModelState.AddModelError("Email", "A participant with this email already exists.");
                return View(participant);
            }
            _context.Participants.Add(participant);
            await _context.SaveChangesAsync();
            TempData["Success"] = $"Participant '{participant.Name}' added successfully!";
            return RedirectToAction(nameof(Index));
        }
        return View(participant);
    }

    public async Task<IActionResult> Edit(int id)
    {
        var participant = await _context.Participants.FindAsync(id);
        if (participant == null) return NotFound();
        return View(participant);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Participant participant)
    {
        if (id != participant.Id) return NotFound();
        ModelState.Remove("Registrations");
        if (ModelState.IsValid)
        {
            _context.Update(participant);
            await _context.SaveChangesAsync();
            TempData["Success"] = $"Participant '{participant.Name}' updated.";
            return RedirectToAction(nameof(Index));
        }
        return View(participant);
    }

    public async Task<IActionResult> Delete(int id)
    {
        var participant = await _context.Participants
            .Include(p => p.Registrations)
            .FirstOrDefaultAsync(p => p.Id == id);
        if (participant == null) return NotFound();
        return View(participant);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var participant = await _context.Participants.FindAsync(id);
        if (participant != null)
        {
            _context.Participants.Remove(participant);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Participant deleted.";
        }
        return RedirectToAction(nameof(Index));
    }
}
