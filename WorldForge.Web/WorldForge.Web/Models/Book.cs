﻿using System.ComponentModel.DataAnnotations;
using WorldForge.Web.Models;

namespace WorldForge.Web.Models
{
    public class Book
    {
        public int Id { get; set; }

        [Display(Name = "Book Title")]
        public string Title { get; set; }

        [Display(Name = "Genre")]
        public BookGenre Genre { get; set; } = BookGenre.Fantasy;

        public string Synopsis { get; set; }

        [Display(Name = "Cover Image URL")]
        [StringLength(512)]
        public string? CoverImageUrl { get; set; }

        [Display(Name = "Part of a Series?")]
        public bool IsSeries { get; set; }

        public int? SeriesId { get; set; } 

        [Display(Name = "Status")]
        public BookStatus Status { get; set; } = BookStatus.Draft;

        [Display(Name = "Created On")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<BookCharacter> BookCharacters { get; set; } = new List<BookCharacter>();
        public ICollection<BookWorldNote> BookWorldNotes { get; set; } = new List<BookWorldNote>();
    }
}
