using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace WorldForge.Web.Models
{
    public class Trait
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public TraitType Type { get; set; }
        public string? Description { get; set; }

        [ValidateNever]
        public ICollection<CharacterTrait> CharacterTraits { get; set; }
    }

}
