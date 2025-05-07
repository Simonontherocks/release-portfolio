using CineVault.ModelLayer.ModelMovie;
using System.Text.Json.Serialization;

namespace CineVault.BusinessLogic.ApiModels
{
    internal class MovieResponse
    {
        [JsonPropertyName("page")]
        public int Page { get; set; }

        [JsonPropertyName("results")]
        public List<Movie> Results { get; set; }

        [JsonPropertyName("total_results")]
        public int TotalResults { get; set; }

        [JsonPropertyName("total_pages")]
        public int TotalPages { get; set; }
    }
}
