using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CineVault.ModelLayer.ModelMovie
{
    /// <summary>
    /// - Tussenklasse `MovieActor'
    /// - Deze klasse verbindt films met acteurs via een tussen-tabel in de database.
    /// - ID (`Id`) als primaire sleutel:
    ///   - Zorgt voor eenvoudige CRUD-operaties (toevoegen, bijwerken, verwijderen).
    ///   - Biedt flexibiliteit voor toekomstige uitbreidingen (zoals rol van de acteur).
    ///   - Betere prestaties en indexering bij databasequeries.
    ///   - Voorkomt complexiteit en moeilijk beheer van een samengestelde sleutel (`MovieId, ActorId`).
    /// - Dit model gebruikt navigatie-eigenschappen om films en acteurs correct te koppelen.
    /// </summary>
    public class MovieActor
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Genereert automatisch een unieke ID
        public int Id { get; set; } // Primaire sleutel voor een unieke koppeling tussen een film en een acteur
        public int MovieId { get; set; } // Foreign key die verwijst naar een film
        public Movie Movie { get; set; } // Navigatie-eigenschap om toegang te krijgen tot de gerelateerde film

        public int ActorId { get; set; } // Foreign key die verwijst naar een acteur
        public Actor Actor { get; set; } // Navigatie-eigenschap om toegang te krijgen tot de gerelateerde acteur

    }

}
