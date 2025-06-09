using CineVault.ModelLayer.ModelMovie;

namespace CineVault.DataAccessLayer.Repositories
{
    /// <summary>
    /// - IMovieActorRepository - Interface voor MovieActor Repository**
    /// - Definieert de contractuele methodes die door `MovieActorRepository` moeten worden geïmplementeerd.
    /// - Waarom een interface?  
    ///   - Zorgt ervoor dat de repository testbaar en flexibel blijft.  
    ///   - Maakt het mogelijk om verschillende implementaties van de repository te gebruiken.  
    /// - Functionaliteiten:  
    ///   - Ophalen, toevoegen, bijwerken en verwijderen van film-acteur relaties.  
    ///   - Beheert database-interacties via Entity Framework Core.  
    /// </summary>

    public interface IMovieActorRepository
    {
        List<MovieActor> GetAll(); // Haalt alle film-acteur koppelingen op uit de database
        MovieActor GetById(int id); // Zoekt een specifieke film-acteur koppeling op basis van ID
        void Insert(MovieActor movieActor); // Voegt een nieuwe film-acteur relatie toe aan de database
        void Update(MovieActor movieActor); // Wijzigt een bestaande film-acteur koppeling
        void Delete(MovieActor movieActor); // Verwijdert een film-acteur koppeling uit de database
    }
}
