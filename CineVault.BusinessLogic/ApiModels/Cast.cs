using System.Text.Json.Serialization;

namespace CineVault.BusinessLogic.ApiModels
{
    public class Cast
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("character")]
        public string Character { get; set; }

        [JsonPropertyName("id")]
        public int Tmdb_Id { get; set; }
    }
}
