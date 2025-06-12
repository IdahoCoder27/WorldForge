using System.Text.Json.Serialization;

namespace WorldForge.Web.Models
{
    public class UnlinkAssociationRequest
    {
        public int Id { get; set; }          // CharacterId or WorldNoteId
        public int BookId { get; set; }      // Required for many-to-many
        public string Type { get; set; }     // "character" or "worldnote"
        public bool Series { get; set; }
    }
}
