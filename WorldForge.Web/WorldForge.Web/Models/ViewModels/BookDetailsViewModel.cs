using System;
using System.Collections.Generic;
using WorldForge.Web.Models;

namespace WorldForge.Web.Models.ViewModels
{
    public class BookDetailsViewModel
    {
        public int Id { get; set; }
        public Book Book { get; set; }
        public string Title { get; set; }
        public string? Genre { get; set; }
        public string? Synopsis { get; set; }
        public bool IsSeries { get; set; }
        public string? Status { get; set; }
        public DateTime CreatedAt { get; set; }

        // New property for the banner image
        public string? CoverImageUrl { get; set; }

        // Associated data
        public List<Character> Characters { get; set; } = new();
        public List<WorldNote> WorldNotes { get; set; } = new();
        public List<Trait> Traits { get; set; } = new();
        public List<Book> OtherBooksInSeries { get; set; } = new();
    }
}
