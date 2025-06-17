using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using WorldForge.Web.Models;

namespace WorldForge.Web.Models.ViewModels
{
    public class EditCharacterViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string CharacterImageUrl { get; set; }

        public IFormFile? CharacterImageFile { get; set; }
        public string? ExistingImageUrl { get; set; }
        //public string CharacterImage { get; set; } // Possibly legacy, may not be needed if replaced by CharacterImageFile
        public string Gender { get; set; }
        public string Race { get; set; }
        public string Role { get; set; }
        public CharacterStatus Status { get; set; }
        public string Backstory { get; set; }


        public List<int> SelectedTraitIds { get; set; } = new();

        public List<Trait> AllTraits { get; set; } = new();

        public List<int> SelectedBookIds { get; set; } = new();

        public List<Book> AllBooks { get; set; } = new();
    }
}
