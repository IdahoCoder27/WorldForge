namespace WorldForge.Web.Models
{
    public class AssociationViewModel
    {
        public List<Book> Books { get; set; }
        public List<Character> UnlinkedCharacters { get; set; }
        public List<WorldNote> UnlinkedWorldNotes { get; set; }
        public List<Trait> UnlinkedCharacterTraits { get; set; } 
    }
}
