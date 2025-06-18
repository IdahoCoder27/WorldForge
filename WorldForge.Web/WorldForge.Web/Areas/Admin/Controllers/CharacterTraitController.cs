using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WorldForge.Web.Data;
using WorldForge.Web.Models;
using WorldForge.Web.Models.ViewModels;

namespace WorldForge.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/[controller]/[action]")]
    [Authorize(Roles = "Admin, Editor, LoreMaster")]
    public class CharacterTraitController : Controller
    {
        private readonly WorldForgeContext _context;

        public CharacterTraitController(WorldForgeContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Assign([FromBody] CharacterTraitAssignRequest request)
        {
            if (request == null || request.CharacterId <= 0 || request.TraitId <= 0)
                return BadRequest("Invalid request");

            // Optional: check if it already exists to avoid duplicates
            var existing = await _context.CharacterTraits
                .FirstOrDefaultAsync(ct => ct.CharacterId == request.CharacterId && ct.TraitId == request.TraitId);

            if (existing != null)
                return Conflict("Trait already assigned to this character.");

            var characterTrait = new CharacterTrait
            {
                CharacterId = request.CharacterId,
                TraitId = request.TraitId,
                CustomDescription = request.CustomDescription
            };

            _context.CharacterTraits.Add(characterTrait);
            await _context.SaveChangesAsync();

            return Ok();
        }

    }
}
