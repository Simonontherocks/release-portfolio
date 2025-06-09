using CineVault.ModelLayer.ModelMovie;
using CineVault.DataAccessLayer.Context;

namespace CineVault.DataAccessLayer.Repositories
{
    public class DirectorRepository : IDirectorRepository
    {
        #region Field

        private readonly AppDBContext _context; // Entity Framework databasecontext voor alle operaties

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor - Injecteert databasecontext
        /// - Dependency Injection (DI) zorgt ervoor dat `AppDBContext` automatisch beschikbaar is.
        /// </summary>
        /// <param name="context"></param>

        public DirectorRepository(AppDBContext context)
        {
            _context = context;
        }

        #endregion

        #region Methods               

        /// <summary>
        /// - Haalt alle regisseurs op uit de database
        /// </summary>
        /// <returns></returns>

        public List<Director> GetAll()
        {
            return _context.Directors.ToList();
        }

        /// <summary>
        /// - Haalt een regisseur op basis van ID
        /// - FirstOrDefault() retourneert `null` als er geen match is.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        public Director GetById(int id)
        {
            return _context.Directors.FirstOrDefault(d => d.Id == id);
        }

        /// <summary>
        /// - Voegt een nieuwe regisseur toe aan de database
        /// - Let op: `SaveChanges()` moet nog worden aangeroepen om de wijziging op te slaan.
        /// </summary>
        /// <param name="director"></param>

        public void Insert(Director director)
        {
            _context.Directors.Add(director);
        }

        /// <summary>
        /// Wijzigt een bestaande regisseur in de database
        /// </summary>
        /// <param name="director"></param>
        /// <exception cref="ArgumentNullException"></exception>

        public void Update(Director director)
        {
            if (director == null)
            {
                throw new ArgumentNullException(nameof(director));
            }
            _context.Directors.Update(director);
        }

        /// <summary>
        /// - Verwijdert een regisseur uit de database
        /// </summary>
        /// <param name="director"></param>
        /// <exception cref="ArgumentNullException"></exception>

        public void Delete(Director director)
        {
            if (director == null)
            {
                throw new ArgumentNullException(nameof(director));
            }

            _context.Directors.Remove(director);
        }

        /// <summary>
        /// - Slaat alle wijzigingen op in de database
        /// - Wordt aangeroepen na Insert, Update of Delete.
        /// </summary>

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        #endregion

    }

}
