using System.Text.Json.Serialization;

namespace CineVault.BusinessLogic.ApiModels
{
    /// <summary>
    /// - Vertegenwoordigt een crew-lid van een film, zoals regisseur of producer, inclusief naam, taak en TMDb-ID.
    /// </summary>

    public class Crew
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } // Naam van het crew-lid

        [JsonPropertyName("job")]
        public string Job { get; set; } // Functie van het crew-lid (bijv. "Director", "Producer")

        [JsonPropertyName("id")]
        public int Tmdb_Id { get; set; } // Unieke ID van het crew-lid in de TMDb-database
    }
}
