using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WorldForge.Web.Models
{
    public class WorldNote
    {
        public int Id { get; set; }

        [Display(Name = "Note Title")]
        public string Title { get; set; }

        [Display(Name = "Category (Lore, Location, etc.)")]
        public string Category { get; set; }

        [Display(Name = "Content")]
        public string Content { get; set; }

        public string? Summary { get; set; }

        [Display(Name = "Created On")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Display(Name = "Book")]
        [ForeignKey("BookId")]
        public int? BookId { get; set; }
        public Book? Book { get; set; }

        public ICollection<BookWorldNote> BookWorldNotes { get; set; }
    }
}
