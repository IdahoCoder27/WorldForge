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
            .Include(c => c.CharacterTraits)
                .ThenInclude(xt => xt.Trait)
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

        var characters = _context.Characters.Where(c => !linkedCharacterIds.Contains(c.Id)).Include(c => c.CharacterTraits)
        .ThenInclude(ct => ct.Trait).ToList();
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
    [HttpPost]
    public async Task<IActionResult> Link([FromBody] AssociationRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest("Invalid request format.");

        var book = await _context.Books
            .Include(b => b.BookCharacters)
            .Include(b => b.BookWorldNotes)
            .FirstOrDefaultAsync(b => b.Id == request.BookId);

        if (book == null)
            return NotFound("Book not found.");

        List<Book> targetBooks = new();

        if (request.Series == true && book.SeriesId != null)
        {
            targetBooks = await _context.Books
                .Include(b => b.BookCharacters)
                .Include(b => b.BookWorldNotes)
                .Where(b => b.SeriesId == book.SeriesId)
                .ToListAsync();
        }
        else
        {
            targetBooks.Add(book);
        }

        switch (request.Type.ToLower())
        {
            case "character":
                var character = await _context.Characters.FindAsync(request.Id);
                if (character == null) return NotFound("Character not found.");

                foreach (var b in targetBooks)
                {
                    if (!b.BookCharacters.Any(x => x.CharacterId == character.Id))
                    {
                        b.BookCharacters.Add(new BookCharacter { BookId = b.Id, CharacterId = character.Id });
                    }
                }
                break;

            case "worldnote":
                var note = await _context.WorldNotes.FindAsync(request.Id);
                if (note == null) return NotFound("World Note not found.");

                foreach (var b in targetBooks)
                {
                    if (!b.BookWorldNotes.Any(x => x.WorldNoteId == note.Id))
                    {
                        b.BookWorldNotes.Add(new BookWorldNote { BookId = b.Id, WorldNoteId = note.Id });
                    }
                }
                break;

            default:
                return BadRequest("Unsupported type.");
        }

        await _context.SaveChangesAsync();
        return Ok(new { message = "Association applied successfully." });
    }


    [HttpPost]
    public async Task<IActionResult> Unlink([FromBody] UnlinkAssociationRequest dto)
    {
        if (dto == null || string.IsNullOrEmpty(dto.Type))
            return BadRequest("Invalid request.");

        switch (dto.Type.ToLower())
        {
            case "character":
                var characterLink = await _context.BookCharacters
                    .FirstOrDefaultAsync(bc => bc.BookId == dto.BookId && bc.CharacterId == dto.Id);

                if (characterLink == null)
                    return NotFound("Character association not found.");

                _context.BookCharacters.Remove(characterLink);
                break;

            case "worldnote":
                var noteLink = await _context.BookWorldNotes
                    .FirstOrDefaultAsync(bn => bn.BookId == dto.BookId && bn.WorldNoteId == dto.Id);

                if (noteLink == null)
                    return NotFound("World Note association not found.");

                _context.BookWorldNotes.Remove(noteLink);
                break;

            default:
                return BadRequest("Unknown type.");
        }

        await _context.SaveChangesAsync();
        return Ok(new { message = "Association removed successfully." });
    }

}
