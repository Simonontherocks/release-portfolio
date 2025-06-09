using CineVault.ModelLayer.ModelMovie;
using System.Text.Json.Serialization;

namespace CineVault.BusinessLogic.ApiModels
{
    /// <summary>
    /// API-responsemodel voor het ophalen van een lijst met films, inclusief paginatie-informatie.
    /// Wordt gebruikt voor endpoints zoals /movie/popular of /search/movie.
    /// </summary>

    public class MovieResponse
    {
        [JsonPropertyName("page")]
        public int Page { get; set; } // Huidige pagina van de resultaten

        [JsonPropertyName("results")]
        public List<Movie> Results { get; set; } // Lijst van films op deze pagina

        [JsonPropertyName("total_results")]
        public int TotalResults { get; set; } // Totaal aantal gevonden resultaten over alle pagina's

        [JsonPropertyName("total_pages")]
        public int TotalPages { get; set; } // Totaal aantal pagina's beschikbaar voor deze query
    }
}
