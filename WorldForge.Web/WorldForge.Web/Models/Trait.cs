namespace WorldForge.Web.Models
{
    public class Trait
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public TraitType Type { get; set; }
        public string Description { get; set; }

        public ICollection<CharacterTrait> CharacterTraits { get; set; }
    }

}
