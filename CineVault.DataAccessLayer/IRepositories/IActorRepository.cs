using CineVault.ModelLayer.ModelMovie;

namespace CineVault.DataAccessLayer.Repositories
{
    /// <summary>
    /// IActorRepository - Interface voor Actor Repository
    /// - Definieert de contractuele methodes die door `ActorRepository` moeten worden geïmplementeerd.
    /// - Waarom een interface?  
    ///   - Zorgt ervoor dat de repository testbaar en flexibel blijft.  
    ///   - Maakt het mogelijk om verschillende implementaties van de repository te gebruiken.  
    /// - Functionaliteiten: 
    ///   - Ophalen, toevoegen, bijwerken en verwijderen van acteurs.  
    ///   - Beheert database-interacties via Entity Framework Core. 
    /// </summary>

    public interface IActorRepository
    {
        List<Actor> GetAll(); // Haalt alle acteurs op uit de database
        Actor GetById(int id); // Zoekt een specifieke acteur op basis van zijn ID
        void Insert(Actor actor); // Voegt een nieuwe acteur toe aan de database
        void Update(Actor actor); // Wijzigt de gegevens van een bestaande acteur
        void Delete(Actor actor); // Verwijdert een acteur uit de database
        void SaveChanges(); // Slaat wijzigingen op in de database na een mutatie
    }
}
