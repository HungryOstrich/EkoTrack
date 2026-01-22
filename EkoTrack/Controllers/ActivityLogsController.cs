using EkoTrack.Data;
using EkoTrack.DTOs;
using EkoTrack.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace EkoTrack.Controllers
{
    [Authorize]
    public class ActivityLogsController : Controller
    {
        private readonly ApplicationDbContext _context;

        private string GetUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
        }
        public ActivityLogsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ActivityLogs
        public async Task<IActionResult> Index()
        {
            var userId = GetUserId();

            var logs = await _context.ActivityLogs
                .Include(a => a.EmissionSource)
                .Include(a => a.EmissionFactor)
                .Where(a => a.CreatedById == userId) 
                .AsNoTracking()
                .ToListAsync();

            var dtos = logs.Select(log => new ActivityLogDTO(log)).ToList();

            return View(dtos);
        }

        // GET: ActivityLogs/Create
        public IActionResult Create()
        {
            PopulateDropDowns();
            return View();
        }

        // POST: ActivityLogs/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
      
        public async Task<IActionResult> Create([Bind("Date,Quantity,EmissionSourceId,EmissionFactorId")] ActivityLog activityLog)
        {
            activityLog.CreatedById = GetUserId();

            ModelState.Remove(nameof(activityLog.CreatedById));
            ModelState.Remove(nameof(activityLog.CreatedBy));
            ModelState.Remove(nameof(activityLog.EmissionFactor));
            ModelState.Remove(nameof(activityLog.EmissionSource));

            var factor = await _context.EmissionFactors.FindAsync(activityLog.EmissionFactorId);

            if (factor != null)
            {
                activityLog.EmissionFactor = factor;
                activityLog.CalculateEmission();
            }
            else
            {
                ModelState.AddModelError("EmissionFactorId", "Nie znaleziono wybranego czynnika emisji.");
            }

            if (ModelState.IsValid)
            {
                _context.Add(activityLog);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            PopulateDropDowns(activityLog.EmissionSourceId, activityLog.EmissionFactorId);
            return View(activityLog);
        }

        // GET: ActivityLogs/Edit
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var activityLog = await _context.ActivityLogs.FindAsync(id);
            if (activityLog == null) return NotFound();

            PopulateDropDowns(activityLog.EmissionSourceId, activityLog.EmissionFactorId);
            return View(activityLog);
        }

        // POST: ActivityLogs/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Date,Quantity,EmissionSourceId,EmissionFactorId")] ActivityLog activityLog)
        {
            if (id != activityLog.Id) return NotFound();

            ModelState.Remove("CreatedById");
            ModelState.Remove("CreatedBy");
            ModelState.Remove("EmissionFactor");
            ModelState.Remove("EmissionSource");

            if (ModelState.IsValid)
            {
                try
                {
                  
                    var existingLog = await _context.ActivityLogs
                        .FirstOrDefaultAsync(x => x.Id == id);

                    if (existingLog == null) return NotFound();

                    existingLog.Date = activityLog.Date;
                    existingLog.Quantity = activityLog.Quantity;
                    existingLog.EmissionSourceId = activityLog.EmissionSourceId;
                    existingLog.EmissionFactorId = activityLog.EmissionFactorId;

              
                    var factor = await _context.EmissionFactors.FindAsync(activityLog.EmissionFactorId);
                    if (factor != null)
                    {
                        existingLog.EmissionFactor = factor;
                        existingLog.CalculateEmission();
                    }

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ActivityLogExists(activityLog.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            PopulateDropDowns(activityLog.EmissionSourceId, activityLog.EmissionFactorId);
            return View(activityLog);
        }

        // GET: ActivityLogs/Delete
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var activityLog = await _context.ActivityLogs
                .Include(a => a.EmissionSource)
                .Include(a => a.EmissionFactor)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (activityLog == null) return NotFound();

         
            var dto = new ActivityLogDTO(activityLog);

            return View(dto);
        }

        // POST: ActivityLogs/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var activityLog = await _context.ActivityLogs.FindAsync(id);
            if (activityLog != null)
            {
                _context.ActivityLogs.Remove(activityLog);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

      
        private void PopulateDropDowns(int? selectedSource = null, int? selectedFactor = null)
        {
            ViewData["EmissionSourceId"] = new SelectList(_context.EmissionSources, "Id", "Name", selectedSource);

            ViewData["EmissionFactorId"] = new SelectList(_context.EmissionFactors.Select(x => new
            {
                Id = x.Id,
                NameWithUnit = $"{x.Name} ({x.Unit})"
            }), "Id", "NameWithUnit", selectedFactor);
        }

        private bool ActivityLogExists(int id)
        {
            return _context.ActivityLogs.Any(e => e.Id == id);
        }
    }
}