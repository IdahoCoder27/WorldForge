using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace WorldForge.Web.Models
{
    public class Character
    {
        public int Id { get; set; }

        [Display(Name = "Full Name")]
        public string Name { get; set; }

        [Display(Name = "Gender Identity")]
        public string Gender { get; set; }

        [Display(Name = "Race / Species")]
        public string Race { get; set; }

        [Display(Name = "Story Role")]
        public string Role { get; set; }

        public string Description { get; set; }

        public CharacterStatus Status { get; set; } = CharacterStatus.Active;

        [Display(Name = "Character Backstory")]
        public string Backstory { get; set; }

        [Display(Name = "Book")]
        public int BookId { get; set; }

        [ValidateNever]
        public Book Book { get; set; }
    }
}
