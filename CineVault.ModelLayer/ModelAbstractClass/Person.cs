using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CineVault.ModelLayer.ModelAbstractClass
{
    public abstract class Person
    {
        /// <summary>
        /// - Dit is een **sjabloon** voor andere klassen zoals `Actor` en `Director`.  
        /// - Zal niet direct geïnstantieerd worden, maar biedt gemeenschappelijke eigenschappen.  
        /// - Voorkomt duplicatie van code door gedeelde eigenschappen centraal te beheren.  
        /// - Waarom abstract? 
        /// Omdat alle personen (acteurs, regisseurs, castingdirectors) dezelfde basis hebben,  
        /// maar hun eigen specifieke eigenschappen en functionaliteiten nodig hebben.
        /// </summary>

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
