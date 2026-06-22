using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CommunityEvents.Data;
using CommunityEvents.Models;

using Microsoft.AspNetCore.Authorization;

namespace CommunityEvents.Controllers;

[Authorize(Roles = "Admin")]
public class ActivitiesController : Controller
{
    private readonly ApplicationDbContext _context;
    public ActivitiesController(ApplicationDbContext context) { _context = context; }

    public async Task<IActionResult> Index()
    {
        var activities = await _context.Activities
            .Include(a => a.EventActivities)
            .ToListAsync();
        return View(activities);
    }

    public async Task<IActionResult> Details(int id)
    {
        var activity = await _context.Activities
            .Include(a => a.EventActivities).ThenInclude(ea => ea.Event)
            .FirstOrDefaultAsync(a => a.Id == id);
        if (activity == null) return NotFound();
        return View(activity);
    }

    public IActionResult Create() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Activity activity)
    {
        ModelState.Remove("EventActivities");
        if (ModelState.IsValid)
        {
            _context.Activities.Add(activity);
            await _context.SaveChangesAsync();
            TempData["Success"] = $"Activity '{activity.Name}' created successfully!";
            return RedirectToAction(nameof(Index));
        }
        return View(activity);
    }

    public async Task<IActionResult> Edit(int id)
    {
        var activity = await _context.Activities.FindAsync(id);
        if (activity == null) return NotFound();
        return View(activity);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Activity activity)
    {
        if (id != activity.Id) return NotFound();
        ModelState.Remove("EventActivities");
        if (ModelState.IsValid)
        {
            _context.Update(activity);
            await _context.SaveChangesAsync();
            TempData["Success"] = $"Activity '{activity.Name}' updated successfully!";
            return RedirectToAction(nameof(Index));
        }
        return View(activity);
    }

    public async Task<IActionResult> Delete(int id)
    {
        var activity = await _context.Activities
            .Include(a => a.EventActivities)
            .FirstOrDefaultAsync(a => a.Id == id);
        if (activity == null) return NotFound();
        return View(activity);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var activity = await _context.Activities.FindAsync(id);
        if (activity != null)
        {
            _context.Activities.Remove(activity);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Activity deleted.";
        }
        return RedirectToAction(nameof(Index));
    }
}
