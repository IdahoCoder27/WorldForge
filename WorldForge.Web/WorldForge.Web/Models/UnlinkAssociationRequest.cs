using System.Text.Json.Serialization;

namespace WorldForge.Web.Models
{
    public class UnlinkAssociationRequest
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }
    }

}
