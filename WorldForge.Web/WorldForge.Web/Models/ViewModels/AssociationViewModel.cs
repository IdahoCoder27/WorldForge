namespace WorldForge.Web.Models.ViewModels
{
    public class AssociationViewModel
    {
        public List<Book> Books { get; set; }
        public List<Character> UnlinkedCharacters { get; set; }
        public List<WorldNote> UnlinkedWorldNotes { get; set; }
        public List<Trait> UnlinkedCharacterTraits { get; set; } 
    }
}
