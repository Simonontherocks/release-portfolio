using System.Text.Json.Serialization;

namespace CineVault.BusinessLogic.ApiModels
{
    public class MovieCreditsResponse
    {
        [JsonPropertyName("cast")]
        public List<Cast> Cast { get; set; }

        [JsonPropertyName("crew")]
        public List<Crew> Crew { get; set; }
    }
}
