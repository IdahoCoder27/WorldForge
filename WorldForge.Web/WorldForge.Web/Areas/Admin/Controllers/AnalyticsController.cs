using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WorldForge.Web.Data;
using WorldForge.Web.Models.ViewModels;
using System.Linq;
using System.Threading.Tasks;

namespace WorldForge.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AnalyticsController : Controller
    {
        private readonly WorldForgeContext _context;

        public AnalyticsController(WorldForgeContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var model = new AnalyticsViewModel
            {
                TotalBooks = await _context.Books.CountAsync(),
                TotalCharacters = await _context.Characters.CountAsync(),
                TotalWorldNotes = await _context.WorldNotes.CountAsync(),

                // Simulated Monthly Active Users (replace with actual login tracking if you have it)
                MonthlyActiveUsers = new[] { 8, 15, 22, 35, 47, 38, 26 }
            };

            return View(model);
        }
    }
}
