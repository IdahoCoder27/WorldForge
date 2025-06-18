namespace WorldForge.Web.Models.ViewModels
{
    public class CharacterTraitAssignRequest
    {
        public int CharacterId { get; set; }
        public int TraitId { get; set; }
        public string? CustomDescription { get; set; }
    }
}
