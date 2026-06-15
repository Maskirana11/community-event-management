using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CommunityEvents.Data;
using CommunityEvents.Models;

namespace CommunityEvents.Controllers;

public class VenuesController : Controller
{
    private readonly ApplicationDbContext _context;
    public VenuesController(ApplicationDbContext context) { _context = context; }

    public async Task<IActionResult> Index()
    {
        var venues = await _context.Venues
            .Include(v => v.EventVenues)
            .ToListAsync();
        return View(venues);
    }

    public async Task<IActionResult> Details(int id)
    {
        var venue = await _context.Venues
            .Include(v => v.EventVenues).ThenInclude(ev => ev.Event)
            .FirstOrDefaultAsync(v => v.Id == id);
        if (venue == null) return NotFound();
        return View(venue);
    }

    public IActionResult Create() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Venue venue)
    {
        ModelState.Remove("EventVenues");
        if (ModelState.IsValid)
        {
            _context.Venues.Add(venue);
            await _context.SaveChangesAsync();
            TempData["Success"] = $"Venue '{venue.Name}' created successfully!";
            return RedirectToAction(nameof(Index));
        }
        return View(venue);
    }

    public async Task<IActionResult> Edit(int id)
    {
        var venue = await _context.Venues.FindAsync(id);
        if (venue == null) return NotFound();
        return View(venue);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Venue venue)
    {
        if (id != venue.Id) return NotFound();
        ModelState.Remove("EventVenues");
        if (ModelState.IsValid)
        {
            _context.Update(venue);
            await _context.SaveChangesAsync();
            TempData["Success"] = $"Venue '{venue.Name}' updated successfully!";
            return RedirectToAction(nameof(Index));
        }
        return View(venue);
    }

    public async Task<IActionResult> Delete(int id)
    {
        var venue = await _context.Venues
            .Include(v => v.EventVenues)
            .FirstOrDefaultAsync(v => v.Id == id);
        if (venue == null) return NotFound();
        return View(venue);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var venue = await _context.Venues.FindAsync(id);
        if (venue != null)
        {
            _context.Venues.Remove(venue);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Venue deleted.";
        }
        return RedirectToAction(nameof(Index));
    }
}
