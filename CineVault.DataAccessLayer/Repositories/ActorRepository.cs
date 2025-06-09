using CineVault.DataAccessLayer.Repositories;
using CineVault.ModelLayer.ModelMovie;
using CineVault.DataAccessLayer.Context;

namespace CineVault.DataAccessLayer
{
    /// <summary>
    /// ActorRepository - Database Repository voor Acteurs
    /// - Deze klasse beheert databaseoperaties voor het `Actor` model.
    /// - Waarom een repository?
    ///   - Zorgt voor een centrale plek om databaseoperaties uit te voeren.
    ///   - Maakt de code flexibel en testbaar door databasefunctionaliteit te scheiden van de business logic. 
    /// - Functionaliteiten:
    ///   - Ophalen, toevoegen, bijwerken en verwijderen van acteurs.  
    ///   - Beheert database-interacties via Entity Framework Core.  
    ///   - Maakt gebruik van DbContext om query's uit te voeren.  
    /// </summary>

    public class ActorRepository : IActorRepository
    {
        #region Field

        private readonly AppDBContext _dbContext; // Entity Framework databasecontext voor alle operaties

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor - Injecteert databasecontext
        /// - Dependency Injection (DI) zorgt ervoor dat `AppDBContext` automatisch beschikbaar is.
        /// </summary>
        /// <param name="context"></param>
        
        public ActorRepository(AppDBContext context)
        {
            _dbContext = context;
        }

        #endregion

        #region Methods        

        /// <summary>
        /// - Haalt alle acteurs op uit de database
        /// </summary>
        /// <returns></returns>

        public List<Actor> GetAll()
        {
            return _dbContext.Actors.ToList();
        }

        /// <summary>
        /// - Haalt een acteur op basis van ID
        /// - FirstOrDefault() retourneert `null` als er geen match is.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        public Actor GetById(int id)
        {
            return _dbContext.Actors.FirstOrDefault(a => a.Id == id); // Zoekt een acteur op basis van ID
        }

        /// <summary>
        /// - Voegt een nieuwe acteur toe aan de database
        /// - Let op: `SaveChanges()` moet nog worden aangeroepen om de wijziging op te slaan.
        /// </summary>
        /// <param name="actor"></param>

        public void Insert(Actor actor)
        {
            _dbContext.Actors.Add(actor); // Voegt de acteur toe aan de `Actors` DbSet
        }

        /// <summary>
        /// Wijzigt een bestaande acteur in de database
        /// </summary>
        /// <param name="actor"></param>

        public void Update(Actor actor)
        {
            _dbContext.Actors.Update(actor); // Past de gegevens van de acteur aan in de database
        }

        /// <summary>
        /// - Verwijdert een acteur uit de database
        /// </summary>
        /// <param name="actor"></param>

        public void Delete(Actor actor)
        {
            _dbContext.Actors.Remove(actor); // Verwijdert de acteur uit de database
        }

        /// <summary>
        /// - Slaat alle wijzigingen op in de database
        /// - Wordt aangeroepen na Insert, Update of Delete.
        /// </summary>

        public void SaveChanges()
        {
            _dbContext.SaveChanges(); // Voert alle wijzigingen uit naar de database
        }        

        #endregion
    }

}
