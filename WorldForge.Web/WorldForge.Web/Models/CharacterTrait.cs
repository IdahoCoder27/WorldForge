namespace WorldForge.Web.Models
{
    public class CharacterTrait
    {
        public int Id { get; set; }
        public int CharacterId { get; set; }
        public Character Character { get; set; }

        public int TraitId { get; set; }
        public Trait Trait { get; set; }
    }
}
