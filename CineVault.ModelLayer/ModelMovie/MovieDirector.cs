using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CineVault.ModelLayer.ModelMovie
{
    /// <summary>
    /// - Tussenklasse `MovieDirector`
    /// - Deze klasse verbindt films met regisseurs via een tussen-tabel in de database.
    /// - ID (`Id`) als primaire sleutel:
    ///   - Maakt CRUD-operaties eenvoudiger (toevoegen, bijwerken, verwijderen).
    ///   - Zorgt voor flexibiliteit, zodat later extra metadata kan worden toegevoegd, zoals het budget van de regisseur of zijn aandeel in de film.
    ///   - Betere indexering en zoekprestaties, waardoor databasequeries sneller verlopen.
    ///   - Voorkomt complexiteit van een samengestelde sleutel (`MovieId, DirectorId`).
    /// - Dit model gebruikt Navigatie-eigenschappen om een film correct te gekoppellen aan één of meerdere regisseurs.
    /// </summary>
    public class MovieDirector
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Genereert automatisch een unieke ID
        public int Id { get; set; } // Primaire sleutel voor een unieke koppeling tussen een film en een regisseur
        public int MovieId { get; set; } // Foreign key die verwijst naar een film
        public Movie movie { get; set; } // Navigatie-eigenschap om toegang te krijgen tot de gerelateerde film

        public int DirectorId { get; set; } // Foreign key die verwijst naar een regisseur
        public Director Director { get; set; } // Navigatie-eigenschap om toegang te krijgen tot de gerelateerde regisseur

    }

}
