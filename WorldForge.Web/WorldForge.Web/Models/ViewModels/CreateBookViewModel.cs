using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;
using WorldForge.Web.Models;

namespace WorldForge.Web.Models.ViewModels
{
    public class CreateBookViewModel
    {
        [Required]
        [Display(Name = "Title")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Genre")]
        public BookGenre Genre { get; set; }

        [Display(Name = "Synopsis")]
        public string? Synopsis { get; set; }

        [Display(Name = "Is this part of a series?")]
        public bool IsSeries { get; set; }

        [Required]
        [Display(Name = "Status")]
        public BookStatus Status { get; set; }

        [Display(Name = "Cover Image")]
        public IFormFile? CoverImageFile { get; set; }

        public string? CoverImageUrl { get; set; }
    }
}
