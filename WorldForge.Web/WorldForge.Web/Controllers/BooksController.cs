using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorldForge.Web.Data;
using WorldForge.Web.Models;
using WorldForge.Web.Models.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;


namespace WorldForge.Web.Controllers
{
    public class BooksController : Controller
    {
        private readonly WorldForgeContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public BooksController(WorldForgeContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Books
        public async Task<IActionResult> Index()
        {
            return View(await _context.Books.ToListAsync());
        }

        // GET: Books/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var book = await _context.Books
                .Include(b => b.BookCharacters)
                    .ThenInclude(bc => bc.Character)
                        .ThenInclude(c => c.CharacterTraits)
                            .ThenInclude(ct => ct.Trait)
                .Include(b => b.BookWorldNotes)
                    .ThenInclude(bn => bn.WorldNote)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (book == null)
                return NotFound();

            // Pull any other books in the same series (excluding current book)
            var otherBooks = new List<Book>();
            if (book.SeriesId != null)
            {
                otherBooks = await _context.Books
                    .Where(b => b.SeriesId == book.SeriesId && b.Id != book.Id)
                    .ToListAsync();
            }

            var viewModel = new BookDetailsViewModel
            {
                Book = book,
                Characters = book.BookCharacters.Select(bc => bc.Character).ToList(),
                WorldNotes = book.BookWorldNotes.Select(bn => bn.WorldNote).ToList(),
                Traits = book.BookCharacters
                    .SelectMany(bc => bc.Character.CharacterTraits)
                    .Select(ct => ct.Trait)
                    .Distinct()
                    .ToList(),
                OtherBooksInSeries = otherBooks,
                CoverImageUrl = book.CoverImageUrl
            };

            return View("Details", viewModel);
        }


        // GET: Books/Create
        public IActionResult Create()
        {
            return View(new CreateBookViewModel());
        }

        // POST: Books/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateBookViewModel model)
        {
            if (ModelState.IsValid)
            {
                string coverImageUrl = null;

                if (model.CoverImageFile != null && model.CoverImageFile.Length > 0)
                {
                    var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images/covers");
                    Directory.CreateDirectory(uploadsFolder); // just in case

                    var fileName = $"{Guid.NewGuid()}{Path.GetExtension(model.CoverImageFile.FileName)}";
                    var filePath = Path.Combine(uploadsFolder, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.CoverImageFile.CopyToAsync(stream);
                    }

                    coverImageUrl = $"/images/covers/{fileName}";
                }

                var book = new Book
                {
                    Title = model.Title,
                    Genre = model.Genre,
                    Synopsis = model.Synopsis,
                    IsSeries = model.IsSeries,
                    Status = model.Status,
                    CreatedAt = DateTime.UtcNow,
                    CoverImageUrl = coverImageUrl ?? "/images/default-cover.png" // fallback
                };

                _context.Add(book);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }


        // GET: Books/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            return View(book);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Book book, IFormFile CoverImageFile)
        {
            if (id != book.Id) return NotFound();

            if (ModelState.IsValid)
            {
                if (CoverImageFile != null && CoverImageFile.Length > 0)
                {
                    var fileName = Path.GetFileNameWithoutExtension(CoverImageFile.FileName);
                    var extension = Path.GetExtension(CoverImageFile.FileName);
                    var newFileName = $"{fileName}_{Guid.NewGuid()}{extension}";
                    var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", "covers", newFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await CoverImageFile.CopyToAsync(stream);
                    }

                    book.CoverImageUrl = $"/images/covers/{newFileName}";
                }

                _context.Update(book);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(book);
        }


        // GET: Books/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .FirstOrDefaultAsync(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book != null)
            {
                _context.Books.Remove(book);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookExists(int id)
        {
            return _context.Books.Any(e => e.Id == id);
        }
    }
}
