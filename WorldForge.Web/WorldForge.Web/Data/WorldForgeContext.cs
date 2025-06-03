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
        public DbSet<BookCharacter> BookCharacters { get; set; }
        public DbSet<BookWorldNote> BookWorldNotes { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // BookCharacter many-to-many join
            modelBuilder.Entity<BookCharacter>()
                .HasKey(bc => new { bc.BookId, bc.CharacterId });

            modelBuilder.Entity<BookCharacter>()
                .HasOne(bc => bc.Book)
                .WithMany(b => b.BookCharacters)
                .HasForeignKey(bc => bc.BookId);

            modelBuilder.Entity<BookCharacter>()
                .HasOne(bc => bc.Character)
                .WithMany(c => c.BookCharacters)
                .HasForeignKey(bc => bc.CharacterId);

            // BookWorldNote many-to-many join
            modelBuilder.Entity<BookWorldNote>()
                .HasKey(bn => new { bn.BookId, bn.WorldNoteId });

            modelBuilder.Entity<BookWorldNote>()
                .HasOne(bn => bn.Book)
                .WithMany(b => b.BookWorldNotes)
                .HasForeignKey(bn => bn.BookId);

            modelBuilder.Entity<BookWorldNote>()
                .HasOne(bn => bn.WorldNote)
                .WithMany(n => n.BookWorldNotes)
                .HasForeignKey(bn => bn.WorldNoteId);
        }

    }
}
