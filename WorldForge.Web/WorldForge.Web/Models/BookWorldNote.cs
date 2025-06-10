namespace WorldForge.Web.Models
{
    public class BookWorldNote
    {
        public int BookId { get; set; }
        public Book Book { get; set; }

        public int WorldNoteId { get; set; }
        public WorldNote WorldNote { get; set; }
    }
}
