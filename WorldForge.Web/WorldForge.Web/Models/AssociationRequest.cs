namespace WorldForge.Web.Models
{
    public class AssociationRequest
    {
        public int Id { get; set; }           // ID of Character or WorldNote
        public int BookId { get; set; }       // ID of Book to associate with
        public string Type { get; set; }      // "character" or "worldnote"
    }
}
