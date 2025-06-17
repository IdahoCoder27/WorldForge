using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace WorldForge.Web.Models.ViewModels
{
    public class CreateCharacterViewModel
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Display(Name = "Description")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Display(Name = "Character Image")]
        public IFormFile ImageFile { get; set; }
    }
}
