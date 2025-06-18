using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WorldForge.Web.Data;
using WorldForge.Web.Models;

namespace WorldForge.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class TraitController : Controller
    {
        private readonly WorldForgeContext _context;

        public TraitController(WorldForgeContext context)
        {
            _context = context;
        }

        // GET: Admin/Trait/Index
        public async Task<IActionResult> Index(string? search)
        {
            var query = _context.Traits.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(t =>
                    t.Name.Contains(search) ||
                    t.Description.Contains(search));
            }

            var traits = await query.OrderBy(t => t.Type).ThenBy(t => t.Name).ToListAsync();
            return View(traits);
        }

        // GET: Admin/Trait/Create
        public IActionResult Create()
        {
            return View("Create");
        }

        // POST: Admin/Trait/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Trait trait)
        {
            if (!ModelState.IsValid)
                return View("Create", trait);

            _context.Traits.Add(trait);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Admin/Trait/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var trait = await _context.Traits.FindAsync(id);
            if (trait == null)
                return NotFound();

            return View("Edit", trait);
        }

        // POST: Admin/Trait/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Trait trait)
        {
            if (id != trait.Id)
                return BadRequest();

            if (!ModelState.IsValid)
                return View("Edit", trait);

            _context.Entry(trait).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Admin/Trait/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var trait = await _context.Traits.FindAsync(id);
            if (trait == null)
                return NotFound();

            _context.Traits.Remove(trait);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var traits = await _context.Traits
                .OrderBy(t => t.Type)
                .ThenBy(t => t.Name)
                .ToListAsync();

            var groupedTraits = traits
                .GroupBy(t => t.Type.ToString()); // Ensure grouping is fully evaluated

            return PartialView("_TraitList", groupedTraits); // ✅ This view must expect IGrouping
        }
    }
}
