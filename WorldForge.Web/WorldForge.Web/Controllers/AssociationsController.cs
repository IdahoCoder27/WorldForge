using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WorldForge.Web.Data;
using WorldForge.Web.Models;

namespace WorldForge.Web.Controllers
{
    public class AssociationsController : Controller
    {
        private readonly WorldForgeContext _context;

        public AssociationsController(WorldForgeContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var books = await _context.Books
                .Include(b => b.Characters)
                .Include(b => b.WorldNotes)
                .ToListAsync();

            var allCharacters = await _context.Characters.ToListAsync();
            var allWorldNotes = await _context.WorldNotes.ToListAsync();

            var unlinkedCharacters = allCharacters.Where(c => c.BookId == null).ToList();
            var unlinkedWorldNotes = allWorldNotes.Where(w => w.BookId == null).ToList();

            var model = new AssociationViewModel
            {
                Books = books,
                UnlinkedCharacters = unlinkedCharacters,
                UnlinkedWorldNotes = unlinkedWorldNotes
            };

            return View("ManageAssociations", model);
        }


        public IActionResult ManageAssociations()
        {
            var books = _context.Books.Include(b => b.Characters).Include(b => b.WorldNotes).ToList();
            var characters = _context.Characters.Where(c => c.BookId == null).ToList();
            var worldNotes = _context.WorldNotes.Where(n => n.BookId == null).ToList();

            var viewModel = new AssociationViewModel
            {
                Books = books,
                UnlinkedCharacters = characters,
                UnlinkedWorldNotes = worldNotes
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AssignCharacterToBook([FromBody] AssociationRequest req)
        {
            var character = await _context.Characters.FindAsync(req.Id);
            if (character == null) return NotFound();
            character.BookId = req.BookId;
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> AssignNoteToBook([FromBody] AssociationRequest req)
        {
            var note = await _context.WorldNotes.FindAsync(req.Id);
            if (note == null) return NotFound();
            note.BookId = req.BookId;
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Link([FromBody] AssociationRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid request format.");

            switch (request.Type.ToLower())
            {
                case "character":
                    var character = await _context.Characters.FindAsync(request.Id);
                    if (character == null) return NotFound("Character not found.");
                    character.BookId = request.BookId;
                    break;

                case "worldnote":
                    var note = await _context.WorldNotes.FindAsync(request.Id);
                    if (note == null) return NotFound("World Note not found.");
                    note.BookId = request.BookId;
                    break;

                default:
                    return BadRequest("Unsupported type.");
            }

            await _context.SaveChangesAsync();
            return Ok(new { message = "Association updated successfully." });
        }
    }
}
