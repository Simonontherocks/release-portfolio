using CineVault.DataAccessLayer.Context;
using CineVault.ModelLayer.ModelMovie;
using Microsoft.EntityFrameworkCore;

namespace CineVault.DataAccessLayer.Repositories
{
    /// <summary>
    /// MovieRepository - Database Repository voor films
    /// - Deze klasse beheert CRUD-operaties en queries voor het `Movie` model.
    /// - Waarom een repository?
    ///   - Zorgt voor een centrale plek om databaseoperaties uit te voeren.
    ///   - Maakt de code flexibel en testbaar door databasefunctionaliteit te scheiden van de business logic.
    /// - Functionaliteiten:
    ///   - Films toevoegen, verwijderen en ophalen.
    ///   - Acteurs en regisseurs koppelen aan films.
    ///   - Filteren op status (gezien/niet gezien).
    /// </summary>
    
    public class MovieRepository : IMovieRepository
    {
        #region Field

        private AppDBContext _dbContext; // Entity Framework databasecontext voor alle operaties

        #endregion

        #region Constructor

        /// <summary>
        /// - Constructor - Injecteert databasecontext
        /// - Dependency Injection (DI) zorgt ervoor dat `AppDBContext` automatisch beschikbaar is.
        /// - Hiermee wordt de `MovieRepository` niet afhankelijk van een vaste database-instantie.
        /// </summary>
        /// <param name="dbContext"></param>

        public MovieRepository(AppDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        #endregion

        #region Check for existing movies

        /// <summary>
        /// - Controleert of een film al bestaat op basis van TMDB ID
        /// - Hiermee wordt voorkomen dat dezelfde film dubbel wordt toegevoegd aan de database.
        /// </summary>
        /// <param name="movie"></param>
        /// <returns></returns>

        public bool CheckIfMovieExists (Movie movie)
        {
            try
            {
                return _dbContext.Movies.Any(m => m.TMDBId.Equals(movie.TMDBId));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in CheckIfMovieExists: {ex.Message}");
                return false; // Voorkomt dat de app crasht bij een onverwachte fout
            }
        }

        #endregion

        #region Adding or removing movies

        /// <summary>
        /// - Voegt een film toe aan de database
        /// </summary>
        /// <param name="movie"></param>

        public void AddMovieByMovie(Movie movie)
        {
            try
            {
                _dbContext.Add(movie); // Voegt de film toe aan de database
                _dbContext.SaveChanges(); // Slaat wijzigingen op
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in AddMovieByMovie: {ex.Message}");
            }
        }

        /// <summary>
        /// - Verwijdert een film uit de database
        /// - Zorgt ervoor dat een film correct wordt verwijderd.
        /// </summary>
        /// <param name="movie"></param>

        public void RemoveMovieByMovie(Movie movie)
        {
            try
            {
                _dbContext.Remove(movie); // Verwijdert de film uit de database
                _dbContext.SaveChanges(); // Slaat wijzigingen op
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in RemoveMovieByMovie: {ex.Message}");
            }
        }

        #endregion

        #region Retrieve movies by status

        /// <summary>
        /// - Haalt alle films op die de gebruiker heeft toegevoegd
        /// - Laat de complete filmlijst zien.
        /// </summary>
        /// <returns></returns>

        public IEnumerable<Movie> ShowAllMoviesAUserContains()
        {
            try
            {
                return _dbContext.Movies.ToList(); // Haalt alle films op uit de database
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in ShowAllMoviesAUserContains: {ex.Message}");
                return Enumerable.Empty<Movie>(); // Voorkomt null errors
            }
        }

        /// <summary>
        /// - Haalt alle films op die de gebruiker heeft gezien
        /// </summary>

        public IEnumerable<Movie> ShowAllMoviesThatHaveBeenSeen()
        {
            try
            {
                return _dbContext.Movies.Where(movie => movie.Seen == true).ToList(); // Filtert op films die als "gezien" zijn gemarkeerd
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in ShowAllMoviesThatHaveBeenSeen: {ex.Message}");
                return Enumerable.Empty<Movie>();
            }
        }

        /// <summary>
        /// - Haalt alle films op die de gebruiker niet heeft gezien
        /// </summary>

        public IEnumerable<Movie> ShowAllMoviesThatHaveNotBeenSeen()
        {
            try
            {
                return _dbContext.Movies.Where(movie => movie.Seen == false).ToList(); // Filtert op films die als "niet-gezien" zijn gemarkeerd
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in ShowAllMoviesThatHaveNotBeenSeen: {ex.Message}");
                return Enumerable.Empty<Movie>();
            }
        }

        #endregion

        #region Filter by movie data

        /// <summary>
        /// - Haalt alle acteurs op die in een specifieke film spelen
        /// - Dit zoekt de acteurs via de tussenklasse `MovieActor`.
        /// </summary>
        /// <param name="movie"></param>
        /// <returns></returns>

        public IEnumerable<Actor> ShowAllActorsFromMovie(Movie movie)
        {
            try
            {
                return _dbContext.MovieActors
                   .Where(ma => ma.MovieId == movie.Id) // Zoek alle records waar de film overeenkomt
                   .Select(ma => ma.Actor) // Haalt de acteurs uit de relatie
                   .ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in ShowAllActorsFromMovie: {ex.Message}");
                return Enumerable.Empty<Actor>();
            }
        }

        /// <summary>
        /// - Haalt alle regisseurs op die een specifieke film hebben geregisseerd
        /// - Dit zoekt de regisseurs via de tussenklasse `MovieDirector`.
        /// </summary>
        /// <param name="movie"></param>
        /// <returns></returns>

        public IEnumerable<Director> ShowDirectorFromMovie(Movie movie)
        {
            try
            {
                return _dbContext.MovieDirectors
                  .Where(md => md.MovieId == movie.Id) // Zoek alle records waar de film overeenkomt
                  .Select(md => md.Director) // Haalt de directors uit de relatie
                  .ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in ShowDirectorFromMovie: {ex.Message}");
                return Enumerable.Empty<Director>();
            }
        }

        /// <summary>
        /// - Haalt alle jaartallen op
        /// </summary>
        /// <param name="movie"></param>
        /// <returns></returns>

        public string ShowYearFromMovie(Movie movie)
        {
            try
            {
                return movie.Year.ToString(); // Haalt alle jaartallen op
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in ShowYearFromMovie: {ex.Message}");
                return string.Empty;
            }
        }

        #endregion

        #region Filter by model

        /// <summary>
        /// - Haalt alle films op waarin een specifieke acteur speelt
        /// </summary>
        /// <param name="actor"></param>
        /// <returns></returns>

        public IEnumerable<Movie> ShowMoviesFromTheSameActor(Actor actor)
        {
            try
            {
                return _dbContext.MovieActors
                        .Where(movieActor => movieActor.Id == actor.Id) // Zoek alle films met deze acteur
                        .Select(movieActor => movieActor.Movie)
                        .ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in ShowMoviesFromTheSameActor: {ex.Message}");
                return Enumerable.Empty<Movie>();
            }
        }

        /// <summary>
        /// - Haalt alle films op die door een specifieke regisseur zijn geregisseerd
        /// </summary>
        /// <param name="director"></param>
        /// <returns></returns>

        public IEnumerable<Movie> ShowMoviesFromTheSameDirector(Director director)
        {
            try
            {
                return _dbContext.MovieDirectors
                        .Where(movieDirector => movieDirector.Id == director.Id) // Zoek alle films met deze regisseur
                        .Select(movieDirector => movieDirector.movie)
                        .ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in ShowMoviesFromTheSameDirector: {ex.Message}");
                return Enumerable.Empty<Movie>();
            }
        }

        /// <summary>
        /// - Haalt alle films op uit een bepaald jaar
        /// </summary>
        /// <param name="strYear"></param>
        /// <returns></returns>

        public IEnumerable<Movie> ShowAllMoviesFromTheSameYear(string strYear)
        {
            try
            {
                if (int.TryParse(strYear, out int year))
                {
                    return _dbContext.Movies
                        .Where(movie => movie.Year == strYear) // Zoek alle films uit hetzelfde jaar.
                        .ToList();
                }

                return Enumerable.Empty<Movie>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in ShowAllMoviesFromTheSameYear: {ex.Message}");
                return Enumerable.Empty<Movie>();
            }
        }

        #endregion

        #region Default CRUD

        /// <summary>
        /// - Haalt een specifieke film op uit de database
        /// - Inclusief acteurs en regisseurs via navigatie-eigenschappen.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        
        public Movie GetById(int id)
        {
            return _dbContext.Movies.Include(m => m.MovieActors) // Hier laad ik de klasse MovieActors vanuit de klasse movie in.
                .ThenInclude(a => a.Actor) // Hier laad ik de acteurs van de film in.
                .Include(m => m.MovieDirectors) // Hier laad ik de klasse MovieDirectors vanuit de klasse movie in.
                .ThenInclude(d => d.Director).FirstOrDefault(m => m.Id == id); // hier laad ik de directors van de film in op basis van de ID.
        }

        #endregion

    }

}
