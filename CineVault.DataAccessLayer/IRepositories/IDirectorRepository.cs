using CineVault.ModelLayer.ModelMovie;

namespace CineVault.DataAccessLayer.Repositories
{
    /// <summary>
    /// IDirectorRepository - Interface voor Director Repository
    /// - Definieert de contractuele methodes die door `DirectorRepository` moeten worden geïmplementeerd.
    /// - Waarom een interface?  
    ///   - Zorgt ervoor dat de repository **testbaar en flexibel** blijft.  
    ///   - Maakt het mogelijk om **verschillende implementaties** van de repository te gebruiken.   
    /// - Functionaliteiten: 
    ///   - Ophalen, toevoegen, bijwerken en verwijderen van acteurs.  
    ///   - Beheert database-interacties via Entity Framework Core. 
    /// </summary>

    public interface IDirectorRepository
    {
        List<Director> GetAll(); // Haalt alle regisseurs op uit de database
        Director GetById(int id); // Zoekt een specifieke regisseur op basis van zijn ID
        void Insert(Director director); // Voegt een nieuwe regisseur toe aan de database
        void Update(Director director); // Wijzigt de gegevens van een bestaande regisseur
        void Delete(Director director); // Verwijdert een regisseur uit de database
        void SaveChanges(); // Slaat wijzigingen op in de database na een mutatie
    }
}
