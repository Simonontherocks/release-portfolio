using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CineVault.BusinessLogic.ApiModels;
using CineVault.DataAccessLayer.Context;
using CineVault.DataAccessLayer.Repositories;
using CineVault.ModelLayer.ModelMovie;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace CineVault.BusinessLogic.Service
{
    /// <summary>
    /// Deze klasse zal contact maken met de DAL.
    /// Deze zal via de methodes hier, data gaan ophalen uit de database.
    /// De methodes in deze klasse hebben dezelfde signatuur als in de DAL, om het leesbaar en eenvoudig te houden.
    /// Er wordt gebruik gemaakt van Dependency Injection in het field, namelijk van deIMovieRepository
    /// </summary>

    public class MovieService
    {
        #region Field

        private readonly IMovieRepository _movieRepository;
        private readonly ApiService _apiService;
        private readonly AppDBContext _appDBContext;

        #endregion

        #region Properties

        public List<Actor> ActorsFromApi { get; set; }
        public List<Director> DirectorsFromApi { get; set; }
        public List<Movie>? MoviesFromApi { get; set; }
        public List<int>? ListWithReceivedIds { get; set; }
        private int MovieId { get; set; }

        #endregion

        #region Constructor

        public MovieService(IMovieRepository movieRepository, ApiService apiService, AppDBContext appDbContext)
        {
            _movieRepository = movieRepository;
            _apiService = apiService;
            _appDBContext = appDbContext;
            ActorsFromApi = new List<Actor>();
            DirectorsFromApi = new List<Director>();
            MoviesFromApi = new List<Movie>();
            ListWithReceivedIds = new List<int>();
        }

        #endregion

        #region Methods

        //#region Check for existing movies

        //public bool MovieExists(string title, string releaseDate)
        //{
        //    return _dbContext.Movies.Any(m => m.Title == title && m.Year == releaseDate);
        //}

        //#endregion

        #region Adding or removing movies

        // Getest in MainProgram => works
        public async Task AddMovieByTitle(string movieTitle)
        {
            bool blCorrectyEnteredId;
            int intEnteredId;
            int intResultId;
            int movieId;
            string strMovieToSearchFor = movieTitle.Trim().ToLower();
            string strEnteredId;
            string strSelectedMovieBasedOnId;
            Movie selectedMovie;

            blCorrectyEnteredId = false;
            intResultId = 0;
            movieId = 0;

            selectedMovie = new Movie();

            if (string.IsNullOrEmpty(strMovieToSearchFor))
            {
                throw new ArgumentNullException(nameof(strMovieToSearchFor), "Movie title cannot be NULL or empty.");
            }

            // Eerst wordt de film opgezocht door de api.

            MoviesFromApi = _apiService.GetMoviesByTitle(strMovieToSearchFor).Result; // Het gevonden resultaat wordt in de property gestoken.

            if (MoviesFromApi is null || !MoviesFromApi.Any()) // Indien de lijst NULL is of dat er geen inzitten, dan wordt er een uitzondering getoont.
            {
                throw new InvalidOperationException("Er zijn geen films gevonden met opgegeven titel");
            }

            // Nu wordt de film gekozen door middel van de IMDB-Id van de film.

            foreach (Movie CurrentMovie in MoviesFromApi)
            {
                Console.WriteLine(CurrentMovie.Title + " " + CurrentMovie.Year + ": " + CurrentMovie.IMDBId);
                ListWithReceivedIds.Add(CurrentMovie.IMDBId);
            }

            while (blCorrectyEnteredId.Equals(false)) // Zolang de ingegeven ID niet correct is, zal de lus opnieuw doorlopen worden.
            {
                Console.WriteLine("Geef de Id in van de film die je wilt toevoegen");
                strEnteredId = Console.ReadLine();

                try
                {
                    intEnteredId = int.Parse(strEnteredId); // de ingegeven ID wordt omgezet naar een integer, omdat de lijst met ID's uit integers bestaat.
                }
                catch
                {
                    throw new Exception("De ingegeven ID mag enkel uit cijfers bestaan"); // Indien er een karakter inzit die geen cijfer is, wordt deze uitzondering getoont.
                }

                if (strEnteredId.IsNullOrEmpty())
                {
                    Console.WriteLine("Je moet een ID ingeven dat in de lijst staat");
                }
                else if (!ListWithReceivedIds.Contains(intEnteredId))
                {
                    Console.WriteLine("Het ingegeven id-nummer staat niet in de lijst. \nProbeer opnieuw");
                }
                else
                {
                    movieId = intEnteredId;
                    blCorrectyEnteredId = true;
                }

            }

            // Nu wordt de juiste film gekozen uit de MoviesFromAPi-lijst

            for (int index = 0; index < MoviesFromApi.Count; index++)
            {
                if (MoviesFromApi[index].IMDBId.Equals(movieId))
                {
                    selectedMovie = MoviesFromApi[index];
                    index = MoviesFromApi.Count;
                }

            }

            // Controle of de film al dan niet in de database zit.
            // Indien de film nog niet in de database zit, zal deze toegevoegd worden

            if (_movieRepository.CheckIfMovieExists(selectedMovie) == false)
            {
                _movieRepository.AddMovieByMovie(selectedMovie);
            }
            else
            {
                Console.WriteLine("Film zit al in database");
            }

            Dictionary<int, Dictionary<string, List<string>>> castAndCrew = await _apiService.GetActorsAndDirectorsFromMovie(movieId);

            if (castAndCrew == null)
            {
                throw new InvalidOperationException("Geen gegevens opgehaald van de API.");
            }

            // Toevoegen van de acteurs en de regisseurs in de database

            foreach (KeyValuePair<int, Dictionary<string, List<string>>> person in castAndCrew)
            {
                int imdbId = person.Key;
                Dictionary<string, List<string>> personInfo = person.Value;

                foreach (KeyValuePair<string, List<string>> nameAndRoles in personInfo)
                {
                    string personName = nameAndRoles.Key;
                    List<string> roles = nameAndRoles.Value;

                    // Acteurs verwerken
                    if (roles.Contains("actor"))
                    {
                        Actor existingActor = _appDBContext.Actors.FirstOrDefault(a => a.Name == personName);
                        if (existingActor == null)
                        {
                            Actor newActor = new Actor { Name = personName, Imdb_ID = imdbId };
                            _appDBContext.Actors.Add(newActor);
                        }
                    }

                    // Regisseurs verwerken
                    if (roles.Contains("director"))
                    {
                        Director existingDirector = _appDBContext.Directors.FirstOrDefault(d => d.Name == personName);
                        if (existingDirector == null)
                        {
                            Director newDirector = new Director { Name = personName, Imdb_ID = imdbId };
                            _appDBContext.Directors.Add(newDirector);
                        }

                    }

                }

            }

            // Wijzigingen opslaan

            _appDBContext.SaveChanges();

            // De relaties tussen Movie, Actor en Director worden gemaakt.

            Movie movie = _appDBContext.Movies.FirstOrDefault(m => m.IMDBId == selectedMovie.IMDBId);

            if (movie == null)
            {
                throw new InvalidOperationException("Film bestaat niet in de database.");
            }

            foreach (KeyValuePair<int, Dictionary<string, List<string>>> person in castAndCrew)
            {
                Dictionary<string, List<string>> personInfo = person.Value;

                foreach (KeyValuePair<string, List<string>> nameAndRoles in personInfo)
                {
                    string personName = nameAndRoles.Key;
                    List<string> roles = nameAndRoles.Value;

                    // Koppelen van Actor aan Movie.
                    if (roles.Contains("actor"))
                    {
                        Actor actor = _appDBContext.Actors.FirstOrDefault(a => a.Name == personName);
                        if (actor != null)
                        {
                            MovieActor movieActor = new MovieActor { MovieId = movie.Id, ActorId = actor.Id };
                            _appDBContext.MovieActors.Add(movieActor);
                        }
                    }

                    // Koppelen van Director aan Movie.
                    if (roles.Contains("director"))
                    {
                        Director director = _appDBContext.Directors.FirstOrDefault(d => d.Name == personName);
                        if (director != null)
                        {
                            MovieDirector movieDirector = new MovieDirector { MovieId = movie.Id, DirectorId = director.Id };
                            _appDBContext.MovieDirectors.Add(movieDirector);
                        }
                    }
                }
            }

            // Relaties opslaan

            _appDBContext.SaveChanges();

        }

        // Getest in MainProgram => works
        public async Task RemoveMovieByIdAsync(int movieId)
        {
            // Controle of het ID geldig is

            if (movieId <= 0)
            {
                throw new ArgumentException("Het opgegeven ID moet een positief geheel getal zijn.", nameof(movieId));
            }

            // Zoek de film in de database op basis van het interne ID
            Movie movieToRemove = await _appDBContext.Movies.FirstOrDefaultAsync(m => m.Id == movieId);

            if (movieToRemove == null)
            {
                throw new InvalidOperationException("Film bestaat niet in de database.");
            }

            // Verwijderen van de relaties met acteurs

            List<MovieActor> movieActors = await _appDBContext.MovieActors
                .Where(ma => ma.MovieId == movieToRemove.Id)
                .ToListAsync();
            _appDBContext.MovieActors.RemoveRange(movieActors);

            // Verwijderen van de relaties met regisseurs
            List<MovieDirector> movieDirectors = await _appDBContext.MovieDirectors
                .Where(md => md.MovieId == movieToRemove.Id)
                .ToListAsync();
            _appDBContext.MovieDirectors.RemoveRange(movieDirectors);

            // Controleer of de acteurs nog in andere films spelen en verwijder ze indien nodig
            foreach (MovieActor movieActor in movieActors)
            {
                Actor actor = await _appDBContext.Actors.FirstOrDefaultAsync(a => a.Id == movieActor.ActorId);

                if (actor != null)
                {
                    bool isActorInOtherMovies = await _appDBContext.MovieActors
                        .AnyAsync(ma => ma.ActorId == actor.Id && ma.MovieId != movieToRemove.Id);

                    if (!isActorInOtherMovies)
                    {
                        _appDBContext.Actors.Remove(actor);
                    }

                }

            }

            // Controle of de regisseurs nog andere films hebben en verwijderen indien dit nodig is.

            foreach (MovieDirector movieDirector in movieDirectors)
            {
                Director director = await _appDBContext.Directors.FirstOrDefaultAsync(d => d.Id == movieDirector.DirectorId);
                if (director != null)
                {
                    bool isDirectorInOtherMovies = await _appDBContext.MovieDirectors
                        .AnyAsync(md => md.DirectorId == director.Id && md.MovieId != movieToRemove.Id);
                    if (!isDirectorInOtherMovies)
                    {
                        _appDBContext.Directors.Remove(director);
                    }

                }

            }

            // Verwijderen van de film.
            _appDBContext.Movies.Remove(movieToRemove);

            // Sla de wijzigingen asynchroon op
            await _appDBContext.SaveChangesAsync();
        }

        #endregion

        #region Retrieve movies by status

        // Getest in MainProgram => works
        public async Task<IEnumerable<Movie>> ShowAllMovies()
        {
            return await _appDBContext.Movies
                .OrderBy(movie => movie.Title)
                .ToListAsync();
        }

        // Getest in MainProgram => works
        public async Task <IEnumerable<Movie>> ShowAllMoviesThatHaveBeenSeen()
        {
            return await _appDBContext.Movies
                .Where(movie => movie.Seen) // Alleen de films die gezien zijn (true)
                .OrderBy(movie => movie.Title)
                .ToListAsync();
        }

        // Getest in MainProgram => works
        public async Task <IEnumerable<Movie>> ShowAllMoviesThatHaveNotBeenSeen()
        {
            return await _appDBContext.Movies
                .Where(movie => !movie.Seen) // Alleen de films die niet gezien zijn (false)
                .OrderBy(movie => movie.Title)
                .ToListAsync();
        }

        #endregion

        #region Filter by movie data

        public async Task<IEnumerable<Actor>> ShowAllActorsFromMovieAsync(Movie movie)
        {
            if (movie == null)
            {
                throw new ArgumentNullException(nameof(movie), "De film mag niet null zijn.");
            }

            return await _appDBContext.MovieActors
                .Where(ma => ma.MovieId == movie.Id)
                .Select(ma => ma.Actor)
                .ToListAsync();
        }

        public async Task<IEnumerable<Director>> ShowDirectorFromMovieAsync(Movie movie)
        {
            if (movie == null)
            {
                throw new ArgumentNullException(nameof(movie), "De film mag niet null zijn.");
            }

            return await _appDBContext.MovieDirectors
                .Where(md => md.MovieId == movie.Id)
                .Select(md => md.Director)
                .ToListAsync();
        }

        public async Task<string> ShowYearFromMovieAsync(Movie movie)
        {
            if (movie == null)
            {
                throw new ArgumentNullException(nameof(movie), "De film mag niet null zijn.");
            }

            return await Task.FromResult(movie.Year.ToString());
        }

        #endregion

        #region Filter by model

        public async Task<IEnumerable<Movie>> ShowMoviesFromTheSameActorAsync(Actor actor)
        {
            if (actor == null)
            {
                throw new ArgumentNullException(nameof(actor), "De acteur mag niet null zijn.");
            }

            bool actorExists = await _appDBContext.Actors.AnyAsync(d => d.Id == actor.Id);
            if (!actorExists)
            {
                throw new InvalidOperationException("De opgegeven acteur bestaat niet in de database.");
            }

            return await _appDBContext.MovieActors
                .Where(ma => ma.ActorId == actor.Id)
                .Select(ma => ma.Movie)
                .OrderBy(m => m.Title)
                .ToListAsync();
        }

        public async Task<IEnumerable<Movie>> ShowMoviesFromTheSameDirectorAsync(Director director)
        {
            if (director == null)
            {
                throw new ArgumentNullException(nameof(director), "De regisseur mag niet null zijn.");
            }

            bool directorExists = await _appDBContext.Directors.AnyAsync(d => d.Id == director.Id);
            if (!directorExists)
            {
                throw new InvalidOperationException("De opgegeven regisseur bestaat niet in de database.");
            }

            return await _appDBContext.MovieDirectors
                .Where(md => md.DirectorId == director.Id)
                .Select(md => md.movie)
                .OrderBy(m => m.Title)
                .ToListAsync();
        }

        // Getest in MainProgram => works
        public async Task<IEnumerable<Movie>> ShowAllMoviesFromTheSameYearAsync(string strYear)
        {
            if (string.IsNullOrWhiteSpace(strYear))
            {
                throw new ArgumentException("Het jaartal mag niet leeg of null zijn.", nameof(strYear));
            }

            return await _appDBContext.Movies
                .Where(m => m.Year == strYear)
                .OrderBy(m => m.Title)
                .ToListAsync();
        }

        #endregion

        #endregion

        #region Methods to avoid duplicat code

        public List<T> ConvertNamesToEntities<T>(Dictionary<string, List<string>> data, string key) where T : new()
        {
            List<T> entities = new List<T>();

            if (data.ContainsKey(key))
            {
                foreach (string name in data[key])
                {
                    T entity = new T();
                    var property = typeof(T).GetProperty("Name");

                    if (property != null)
                    {
                        property.SetValue(entity, name);
                    }

                    entities.Add(entity);
                }
            }

            return entities;
        }

        #endregion

    }
}
