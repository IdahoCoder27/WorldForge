using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WorldForge.Web.Data;
using WorldForge.Web.Models;

namespace WorldForge.Web.Controllers
{
    public class WorldNotesController : Controller
    {
        private readonly WorldForgeContext _context;

        public WorldNotesController(WorldForgeContext context)
        {
            _context = context;
        }

        // GET: WorldNotes
        public async Task<IActionResult> Index()
        {
            var worldForgeContext = _context.WorldNotes.Include(w => w.Book);
            return View(await worldForgeContext.ToListAsync());
        }

        // GET: WorldNotes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var worldNote = await _context.WorldNotes
                .Include(w => w.Book)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (worldNote == null)
            {
                return NotFound();
            }

            return View(worldNote);
        }

        // GET: WorldNotes/Create
        public IActionResult Create()
        {
            ViewData["BookId"] = new SelectList(_context.Books, "Id", "Id");
            return View(new WorldNote());
        }

        // POST: WorldNotes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(WorldNote worldNote)
        {
            if (ModelState.IsValid)
            {
                _context.Add(worldNote);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BookId"] = new SelectList(_context.Books, "Id", "Id", worldNote.BookId);
            return View(worldNote);
        }

        // GET: WorldNotes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var worldNote = await _context.WorldNotes.FindAsync(id);
            if (worldNote == null)
            {
                return NotFound();
            }
            ViewData["BookId"] = new SelectList(_context.Books, "Id", "Id", worldNote.BookId);
            return View(worldNote);
        }

        // POST: WorldNotes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, WorldNote worldNote)
        {
            if (id != worldNote.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(worldNote);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WorldNoteExists(worldNote.Id))
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
            ViewData["BookId"] = new SelectList(_context.Books, "Id", "Id", worldNote.BookId);
            return View(worldNote);
        }

        // GET: WorldNotes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var worldNote = await _context.WorldNotes
                .Include(w => w.Book)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (worldNote == null)
            {
                return NotFound();
            }

            return View(worldNote);
        }

        // POST: WorldNotes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var worldNote = await _context.WorldNotes.FindAsync(id);
            if (worldNote != null)
            {
                _context.WorldNotes.Remove(worldNote);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WorldNoteExists(int id)
        {
            return _context.WorldNotes.Any(e => e.Id == id);
        }
    }
}
