using System.Collections.Generic;
using WorldForge.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace WorldForge.Web.Data
{
    public class WorldForgeContext : DbContext
    {
        public WorldForgeContext(DbContextOptions<WorldForgeContext> options) : base(options) { }

        public DbSet<Book> Books { get; set; }
        public DbSet<Character> Characters { get; set; }
        public DbSet<WorldNote> WorldNotes { get; set; }
    }
}
