using CineVault.ModelLayer.ModelMovie;

namespace CineVault.DataAccessLayer
{
    /// <summary>
    /// - IMovieDirectorRepository - Interface voor MovieDirector Repository**
    /// - Definieert de contractuele methodes die door `MovieDirectorRepository` moeten worden geïmplementeerd.
    /// - Waarom een interface?  
    ///   - Zorgt ervoor dat de repository testbaar en flexibel blijft.  
    ///   - Maakt het mogelijk om verschillende implementaties van de repository te gebruiken.  
    /// - Functionaliteiten:  
    ///   - Ophalen, toevoegen, bijwerken en verwijderen van film-acteur relaties.  
    ///   - Beheert database-interacties via Entity Framework Core.  
    /// </summary>

    public interface IMovieDirectorRepository
    {
        List<MovieDirector> GetAll(); // Haalt alle film-regisseur koppelingen op uit de database
        MovieDirector GetById(int id); // Zoekt een specifieke film-regisseur koppeling op basis van ID
        void Insert(MovieDirector movieDirector); // Voegt een nieuwe film-regisseur relatie toe aan de database
        void Update(MovieDirector movieDirector); // Wijzigt een bestaande film-regisseur koppeling
        void Delete(MovieDirector movieDirector); // Verwijdert een film-regisseur koppeling uit de database
    }
}
