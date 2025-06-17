using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorldForge.Web.Data;
using WorldForge.Web.Models;
using WorldForge.Web.Models.ViewModels;

namespace WorldForge.Web.Controllers
{
    public class CharactersController : Controller
    {
        private readonly WorldForgeContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public CharactersController(WorldForgeContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Characters
        public async Task<IActionResult> Index()
        {
            var worldForgeContext = _context.Characters.Include(c => c.Book);
            return View(await worldForgeContext.ToListAsync());
        }

        // GET: Characters/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var character = await _context.Characters
                .Include(c => c.CharacterTraits)
                    .ThenInclude(ct => ct.Trait)
                .Include(c => c.BookCharacters)
                    .ThenInclude(bc => bc.Book)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (character == null)
                return NotFound();

            var viewModel = new CharacterDetailsViewModel
            {
                Id = character.Id,
                Name = character.Name,
                Description = character.Description,
                Gender = character.Gender,
                Race = character.Race,
                Role = character.Role,
                Backstory = character.Backstory,
                Status = character.Status,
                CharacterImageUrl = character.CharacterImageUrl,
                Books = character.BookCharacters?.Select(bc => bc.Book).ToList(),
                CharacterTraits = character.CharacterTraits?.ToList()
            };

            return View(viewModel);
        }

        // GET: Characters/Create
        public IActionResult Create()
        {
            var books = _context.Books.OrderBy(b => b.Title).ToList();
            ViewBag.BookId = new SelectList(books, "Id", "Title");
            ViewBag.NoBooksExist = !books.Any();
            return View();
        }

        // POST: Characters/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateCharacterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            string imageUrl = "/images/default-character.jpg"; // fallback/default image

            if (model.ImageFile != null && model.ImageFile.Length > 0)
            {
                // Ensure the uploads/characters directory exists
                var uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "characters");
                if (!Directory.Exists(uploadDir))
                {
                    Directory.CreateDirectory(uploadDir);
                }

                var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(model.ImageFile.FileName);
                var filePath = Path.Combine(uploadDir, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.ImageFile.CopyToAsync(stream);
                }

                imageUrl = "/uploads/characters/" + uniqueFileName;
            }

            var character = new Character
            {
                Name = model.Name,
                Description = model.Description,
                CharacterImageUrl = imageUrl
            };

            _context.Add(character);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        // GET: Characters/Edit/5
        // GET: Characters/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var character = await _context.Characters
                .Include(c => c.CharacterTraits)
                    .ThenInclude(ct => ct.Trait)
                .Include(c => c.BookCharacters)
                    .ThenInclude(bc => bc.Book)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (character == null)
                return NotFound();

            var viewModel = new EditCharacterViewModel
            {
                Id = character.Id,
                Name = character.Name,
                Gender = character.Gender,
                Race = character.Race,
                Role = character.Role,
                Status = character.Status,
                Description = character.Description,
                Backstory = character.Backstory,
                CharacterImageUrl = character.CharacterImageUrl,
                ExistingImageUrl = character.CharacterImageUrl,
                SelectedBookIds = character.BookCharacters?.Select(bc => bc.BookId).ToList(),
                SelectedTraitIds = character.CharacterTraits?.Select(ct => ct.TraitId).ToList()
            };

            return View(viewModel);
        }



        // POST: Characters/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditCharacterViewModel viewModel)
        {
            if (id != viewModel.Id) return NotFound();

            if (!ModelState.IsValid) return View(viewModel);

            var character = await _context.Characters
                .Include(c => c.CharacterTraits)
                .Include(c => c.BookCharacters)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (character == null) return NotFound();

            // Update basic fields
            character.Name = viewModel.Name;
            character.Gender = viewModel.Gender;
            character.Race = viewModel.Race;
            character.Role = viewModel.Role;
            character.Status = viewModel.Status;
            character.Description = viewModel.Description;
            character.Backstory = viewModel.Backstory;

            // Handle image upload
            if (viewModel.CharacterImageFile != null && viewModel.CharacterImageFile.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(viewModel.CharacterImageFile.FileName);
                var imagePath = Path.Combine("wwwroot", "images", "characters", fileName);

                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    await viewModel.CharacterImageFile.CopyToAsync(stream);
                }

                character.CharacterImageUrl = "/images/characters/" + fileName;
            }
            else
            {
                character.CharacterImageUrl = viewModel.CharacterImageUrl;
            }

            // === Handle Traits ===
            character.CharacterTraits.Clear();
            if (viewModel.SelectedTraitIds != null && viewModel.SelectedTraitIds.Any())
            {
                var traits = await _context.CharacterTraits
                    .Where(t => viewModel.SelectedTraitIds.Contains(t.TraitId))
                    .ToListAsync();

                character.CharacterTraits = traits;
            }

            // === Handle Books ===
            character.BookCharacters.Clear();
            if (viewModel.SelectedBookIds != null && viewModel.SelectedBookIds.Any())
            {
                character.BookCharacters = viewModel.SelectedBookIds
                    .Select(bookId => new BookCharacter
                    {
                        BookId = bookId,
                        CharacterId = character.Id
                    }).ToList();
            }

            _context.Update(character);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }



        // GET: Characters/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var character = await _context.Characters
                .Include(c => c.Book)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (character == null)
            {
                return NotFound();
            }

            return View(character);
        }

        // POST: Characters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var character = await _context.Characters.FindAsync(id);
            if (character != null)
            {
                _context.Characters.Remove(character);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CharacterExists(int id)
        {
            return _context.Characters.Any(e => e.Id == id);
        }
    }
}
