using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WorldForge.Web.Data;
using WorldForge.Web.Models;

[ApiController]
[Route("[controller]/[action]")]
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
            .Include(b => b.BookCharacters).ThenInclude(bc => bc.Character)
            .Include(b => b.BookWorldNotes).ThenInclude(bn => bn.WorldNote)
            .ToListAsync();

        var linkedCharacterIds = books.SelectMany(b => b.BookCharacters.Select(bc => bc.CharacterId)).ToHashSet();
        var linkedNoteIds = books.SelectMany(b => b.BookWorldNotes.Select(bn => bn.WorldNoteId)).ToHashSet();

        var unlinkedCharacters = await _context.Characters
            .Where(c => !linkedCharacterIds.Contains(c.Id))
            .ToListAsync();

        var unlinkedWorldNotes = await _context.WorldNotes
            .Where(n => !linkedNoteIds.Contains(n.Id))
            .ToListAsync();

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
        var books = _context.Books
            .Include(b => b.BookCharacters).ThenInclude(bc => bc.Character)
            .Include(b => b.BookWorldNotes).ThenInclude(bn => bn.WorldNote)
            .ToList();

        var linkedCharacterIds = books.SelectMany(b => b.BookCharacters.Select(bc => bc.CharacterId)).ToHashSet();
        var linkedNoteIds = books.SelectMany(b => b.BookWorldNotes.Select(bn => bn.WorldNoteId)).ToHashSet();

        var characters = _context.Characters.Where(c => !linkedCharacterIds.Contains(c.Id)).ToList();
        var worldNotes = _context.WorldNotes.Where(n => !linkedNoteIds.Contains(n.Id)).ToList();

        var viewModel = new AssociationViewModel
        {
            Books = books,
            UnlinkedCharacters = characters,
            UnlinkedWorldNotes = worldNotes
        };

        return View(viewModel);
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
                var existingCharLink = await _context.BookCharacters
                    .FirstOrDefaultAsync(bc => bc.BookId == request.BookId && bc.CharacterId == request.Id);
                if (character == null) return NotFound("Character not found.");
                if (existingCharLink == null)
                {
                    _context.BookCharacters.Add(new BookCharacter
                    {
                        BookId = (int)request.BookId,
                        CharacterId = request.Id
                    });
                }
                break;

            case "worldnote":
                var note = await _context.WorldNotes.FindAsync(request.Id);
                var existingNoteLink = await _context.BookWorldNotes
                    .FirstOrDefaultAsync(bn => bn.BookId == request.BookId && bn.WorldNoteId == request.Id);
                if (note == null) return NotFound("World Note not found.");
                if (existingNoteLink == null)
                {
                    _context.BookWorldNotes.Add(new BookWorldNote
                    {
                        BookId = (int)request.BookId,
                        WorldNoteId = request.Id
                    });
                }
                break;

            default:
                return BadRequest("Unsupported type.");
        }

        await _context.SaveChangesAsync();
        return Ok(new { message = "Association created successfully." });
    }

    [HttpPost]
    public async Task<IActionResult> Unlink([FromBody] AssociationRequest dto)
    {
        if (dto == null || string.IsNullOrEmpty(dto.Type))
            return BadRequest("Invalid request.");

        switch (dto.Type.ToLower())
        {
            case "character":
                var charLink = await _context.BookCharacters
                    .FirstOrDefaultAsync(bc => bc.BookId == dto.BookId && bc.CharacterId == dto.Id);
                if (charLink != null)
                    _context.BookCharacters.Remove(charLink);
                break;

            case "worldnote":
                var noteLink = await _context.BookWorldNotes
                    .FirstOrDefaultAsync(bn => bn.BookId == dto.BookId && bn.WorldNoteId == dto.Id);
                if (noteLink != null)
                    _context.BookWorldNotes.Remove(noteLink);
                break;

            default:
                return BadRequest("Unknown type.");
        }

        await _context.SaveChangesAsync();
        return Ok(new { message = "Association removed successfully." });
    }
}
