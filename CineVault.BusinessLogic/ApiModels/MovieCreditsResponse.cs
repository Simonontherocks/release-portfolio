using System.Text.Json.Serialization;

namespace CineVault.BusinessLogic.ApiModels
{
    /// <summary>
    /// - API-responsemodel dat zowel de cast als de crew van een film bevat.
    /// - Wordt gebruikt om gegevens op te halen van TMDb /movie/{id}/credits.
    /// </summary>

    public class MovieCreditsResponse
    {
        [JsonPropertyName("cast")]
        public List<Cast> Cast { get; set; } // Lijst van cast-leden (acteurs en hun rollen)

        [JsonPropertyName("crew")]
        public List<Crew> Crew { get; set; } // Lijst van crew-leden (zoals regisseurs, schrijvers, etc.)
    }
}
