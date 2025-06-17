using System.Collections.Generic;
using WorldForge.Web.Models;

namespace WorldForge.Web.Models.ViewModels
{
    public class CharacterDetailsViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string? Description { get; set; }

        public string? CharacterImageUrl { get; set; }
        public string? Gender { get; set; }
        public string? Race { get; set; }
        public string? Role { get; set; }
        public CharacterStatus Status { get; set; }
        public string? Backstory { get; set; }


        public List<Book>? Books { get; set; }

        public List<CharacterTrait>? CharacterTraits { get; set; }
    }
}
