using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CineVault.ModelLayer.ModelAbstractClass
{
    public abstract class Person
    {
        /*
         * Hier wordt de abstracte klasse gemaakt.
         * Dit zal dienen als een sjabloon voor de andere klassen.
         * Het voordeel is dat het in de toekomst herbruikbaar is.
         */

        #region Properties

        [Key]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        [Display(Name = "Naam")]
        public string Name { get; set; }

        [JsonPropertyName("id")]
        public int Tmdb_ID { get; set; } // Hier zal de tmdb-id van de desbetreffende persoon opgeslaan worden

        #endregion

    }

}
