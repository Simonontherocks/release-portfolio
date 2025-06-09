using System.Text.Json.Serialization;

namespace CineVault.BusinessLogic.ApiModels
{
    /// <summary>
    /// - Vertegenwoordigt een individuele cast-member van een film, inclusief naam, rol en TMDb-ID.
    /// </summary>

    public class Cast
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } // Naam van de acteur

        [JsonPropertyName("character")]
        public string Character { get; set; } // Naam van het personage gespeeld door de acteur

        [JsonPropertyName("id")]
        public int Tmdb_Id { get; set; } // Unieke ID van de cast-lid in de TMDb-database
    }
}
