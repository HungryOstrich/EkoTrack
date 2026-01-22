using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EkoTrack.Data;
using EkoTrack.Models;
using EkoTrack.DTOs; // Added namespace for DTOs

namespace EkoTrack.Controllers
{
    public class OrganizationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrganizationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Organizations
        public async Task<IActionResult> Index()
        {
            // Fetch entities including Users to calculate the count
            var organizations = await _context.Organizations
                .Include(o => o.Users)
                .ToListAsync();

            // Map Entity list to DTO list
            var dtos = organizations.Select(o => new OrganizationDTO(o)).ToList();

            return View(dtos);
        }

        // GET: Organizations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var organization = await _context.Organizations
                .Include(o => o.Users)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (organization == null) return NotFound();

            // Return DTO to the View
            return View(new OrganizationDTO(organization));
        }

        // GET: Organizations/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Organizations/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(OrganizationDTO organizationDto)
        {
            if (ModelState.IsValid)
            {
                // Map DTO -> Entity
                var organization = new Organization
                {
                    Name = organizationDto.Name,
                    TaxId = organizationDto.TaxId,
                    Address = organizationDto.Address
                };

                _context.Add(organization);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(organizationDto);
        }

        // GET: Organizations/Edit
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var organization = await _context.Organizations.FindAsync(id);
            if (organization == null) return NotFound();

           
            return View(new OrganizationDTO(organization));
        }

        // POST: Organizations/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, OrganizationDTO organizationDto)
        {
            if (id != organizationDto.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                   
                    var organizationToUpdate = await _context.Organizations.FindAsync(id);

                    if (organizationToUpdate == null) return NotFound();

                    
                    organizationToUpdate.Name = organizationDto.Name;
                    organizationToUpdate.TaxId = organizationDto.TaxId;
                    organizationToUpdate.Address = organizationDto.Address;

                    

                    _context.Update(organizationToUpdate);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrganizationExists(organizationDto.Id))
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
            return View(organizationDto);
        }

        // GET: Organizations/Delete
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var organization = await _context.Organizations
                .Include(o => o.Users)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (organization == null) return NotFound();

            return View(new OrganizationDTO(organization));
        }

        // POST: Organizations/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var organization = await _context.Organizations.FindAsync(id);
            if (organization != null)
            {
                _context.Organizations.Remove(organization);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrganizationExists(int id)
        {
            return _context.Organizations.Any(e => e.Id == id);
        }
    }
}