using System.Text.Json.Serialization;

namespace WorldForge.Web.Models
{
    public class AssociationRequest
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("bookId")]
        public int? BookId { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("series")]
        public bool? Series { get; set; }
    }
}
