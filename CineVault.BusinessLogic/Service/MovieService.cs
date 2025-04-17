using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Reflection;
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

        #region Adding or removing movies

        [Obsolete] // was goed om de api te leren kennen, maar zal niet gebruikt worden omdat deze in de program getest is geweest.
        // Getest in MainProgram => works
        public async Task AddMovieByTitle(string movieTitle, bool seen)
        {
            bool blCorrectyEnteredId;
            int intEnteredId;
            int intResultId;
            int intMovieId;
            string strEnteredId;
            string strMovieToSearchFor;            
            string strSelectedMovieBasedOnId;
            Movie selectedMovie;

            blCorrectyEnteredId = false;
            intResultId = 0;
            intMovieId = 0;
            selectedMovie = new Movie();

            if (string.IsNullOrWhiteSpace(movieTitle))
            {
                throw new ArgumentNullException("Titel mag niet null of leeg zijn", nameof(movieTitle));
            }
            else
            {
                strMovieToSearchFor = movieTitle.Trim().ToLower();
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
                    intMovieId = intEnteredId;
                    blCorrectyEnteredId = true;
                }

            }

            // Nu wordt de juiste film gekozen uit de MoviesFromAPi-lijst

            for (int index = 0; index < MoviesFromApi.Count; index++)
            {
                if (MoviesFromApi[index].IMDBId.Equals(intMovieId))
                {
                    selectedMovie = MoviesFromApi[index];
                    index = MoviesFromApi.Count;
                }

            }

            // Controle of de film al dan niet in de database zit.
            // Indien de film nog niet in de database zit, zal deze toegevoegd worden

            if (_movieRepository.CheckIfMovieExists(selectedMovie) == false)
            {
                selectedMovie.Seen = seen;  // Zet de film direct op Seen of NotSeen
                _movieRepository.AddMovieByMovie(selectedMovie);
            }
            else
            {
                Console.WriteLine("Film zit al in database");
            }

            Dictionary<int, Dictionary<string, List<string>>> castAndCrew = await _apiService.AddMovieWithActorsAndDirectorsToDatabase(intMovieId);

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

        public async Task AddMovieByImdbId(int imdbId)
        {
            bool blCorrectyEnteredId;
            int intEnteredId;
            int intResultId;
            int intMovieId;
            string strEnteredId;
            string strMovieToSearchFor;
            string strSelectedMovieBasedOnId;
            Movie selectedMovie;

            blCorrectyEnteredId = false;
            intResultId = 0;
            intMovieId = 0;
            selectedMovie = new Movie();

            if (imdbId == 0)
            {
                throw new ArgumentNullException("imdb-ID mag niet null zijn", nameof(imdbId));
            }

            // Eerst wordt de film opgezocht door de api.

            //ToDO GetMovieByImdbId in apiService aanmaken
            selectedMovie = await _apiService.GetMovieByImdbId(imdbId); // Het gevonden resultaat wordt in de property gestoken.

            if (selectedMovie is null) // Indien er geen film gevonden is, zal er een uitzondering getoont worden.
            {
                throw new InvalidOperationException("Er zijn geen films gevonden met opgegeven ID");
            }


            // Controle of de film al dan niet in de database zit.
            // Indien de film nog niet in de database zit, zal deze toegevoegd worden

            if (_movieRepository.CheckIfMovieExists(selectedMovie) == false)
            {
                selectedMovie.Seen = false;  // Zet de film direct op Seen of NotSeen
                _movieRepository.AddMovieByMovie(selectedMovie);
            }
            else
            {
                Console.WriteLine("Film zit al in database");
            }

            Dictionary<int, Dictionary<string, List<string>>> castAndCrew = await _apiService.AddMovieWithActorsAndDirectorsToDatabase(imdbId);

            if (castAndCrew == null)
            {
                throw new InvalidOperationException("Geen gegevens opgehaald van de API.");
            }

            // Toevoegen van de acteurs en de regisseurs in de database

            foreach (KeyValuePair<int, Dictionary<string, List<string>>> person in castAndCrew)
            {
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
                            Actor newActor = new Actor { Name = personName, Imdb_ID = person.Key };
                            _appDBContext.Actors.Add(newActor);
                        }
                    }

                    // Regisseurs verwerken
                    if (roles.Contains("director"))
                    {
                        Director existingDirector = _appDBContext.Directors.FirstOrDefault(d => d.Name == personName);
                        if (existingDirector == null)
                        {
                            Director newDirector = new Director { Name = personName, Imdb_ID = person.Key };
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

        // Getest in MainProgram => works
        public async Task<IEnumerable<Actor>> ShowAllActorsFromMovieAsync(string partialMovieTitle)
        {
            Movie selectedMovie = await GetMovieByPartialTitleAsync(partialMovieTitle);

            if (partialMovieTitle == null)
            {
                throw new ArgumentNullException(nameof(partialMovieTitle), "De film mag niet null zijn.");
            }

            return await _appDBContext.MovieActors
                .Where(ma => ma.MovieId == selectedMovie.Id)
                .Select(ma => ma.Actor)
                .ToListAsync();
        }

        // Getest in MainProgram => works
        public async Task<IEnumerable<Director>> ShowDirectorFromMovieAsync(string partialMovieTitle)
        {
            Movie selectedMovie = await GetMovieByPartialTitleAsync(partialMovieTitle);

            if (partialMovieTitle == null)
            {
                throw new ArgumentNullException(nameof(partialMovieTitle), "De film mag niet null zijn.");
            }

            return await _appDBContext.MovieDirectors
                .Where(md => md.MovieId == selectedMovie.Id)
                .Select(md => md.Director)
                .ToListAsync();
        }

        // Getest in MainProgram => works
        public async Task<string> ShowYearFromMovieAsync(string partialMovieTitle)
        {
            Movie selectedMovie = await GetMovieByPartialTitleAsync(partialMovieTitle);

            if (partialMovieTitle == null)
            {
                throw new ArgumentNullException(nameof(partialMovieTitle), "De film mag niet null zijn.");
            }

            if (selectedMovie == null)
            {
                throw new ArgumentException($"geen film gevonden met titel: {partialMovieTitle}");
            }

            return selectedMovie.Year;
        }

        #endregion

        #region Filter by model

        // Getest in MainProgram => works
        public async Task<IEnumerable<Movie>> ShowMoviesFromTheSameActorAsync(Actor actor)
        {
            if (actor == null || string.IsNullOrWhiteSpace(actor.Name))
            {
                throw new ArgumentNullException(nameof(actor), "De acteur mag niet null zijn.");
            }

            Actor matchedActor = await GetActorByPartialNameAsync(actor.Name);
            if (matchedActor == null)
            {
                throw new InvalidOperationException($"Geen acteur gevonden met de naam {actor.Name}.");
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

        // Getest in MainProgram => works
        public async Task<IEnumerable<Movie>> ShowMoviesFromTheSameDirectorAsync(Director director)
        {
            if (director == null || string.IsNullOrWhiteSpace(director.Name))
            {
                throw new ArgumentNullException(nameof(director), "De regisseur mag niet null zijn.");
            }

            Director matchedDirector = await GetDirectorByPartialNameAsync(director.Name);
            if (matchedDirector == null)
            {
                throw new InvalidOperationException($"Geen acteur gevonden met de naam {director.Name}.");
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

            if(CheckForCharacters(strYear).Equals(false))
            {
                throw new ArgumentException("Het jaartal moet uit enkel karakters bestaan.", nameof(strYear));
            }

            if(strYear.Length != 4)
            {
                throw new ArgumentException("Het jaartal moet uit 4 cijfers bestaan.", nameof(strYear));
            }

            return await _appDBContext.Movies
                .Where(m => m.Year == strYear)
                .OrderBy(m => m.Title)
                .ToListAsync();
        }

        // Added as extra
        public List<string?> GetAllYears()
        {
            return _appDBContext.Movies.Select(m => m.Year)
                .Order()
                .Distinct()
                .ToList();
        }

        public Movie GetById(int id)
        {
            // Eerste controle is om te kijken of de Id een geldige waarde heeft.
            // Indien de waarde 0 is, dan wordt er een uitzondering getoont.
            if (id.Equals(0))
            {
                throw new ArgumentException("ongeldige id");
            }

            Movie requestedMovie = _movieRepository.GetById(id); // hier wordt de director opgehaald via de repository uit de database.

            // Hier gebeurd de controle op de opgevraagde director, om na te gaan of die niet null is.
            // Indien deze null is, dan wordt er een uitzondering getoond.
            if (requestedMovie == null)
            {
                throw new ArgumentNullException(nameof(requestedMovie));
            }

            // De opgevraagde director wordt teruggegeven.
            return requestedMovie;
        }

        #endregion

        #region Changing status

        /// <summary>
        /// Stelt de 'Seen'-status in voor een film op basis van het film-ID.
        /// </summary>
        /// <param name="movieId">Het unieke ID van de film.</param>
        /// <param name="seen">De nieuwe waarde voor de 'Seen'-status (true = gezien, false = niet gezien).</param>
        /// <exception cref="ArgumentException">Wanneer het ID ongeldig is (bijv. 0 of negatief).</exception>
        /// <exception cref="InvalidOperationException">Wanneer de film niet wordt gevonden in de database.</exception>
        public async Task SetMovieSeenStatusAsync(int movieId, bool seen)
        {
            if (movieId <= 0)
            {
                throw new ArgumentException("Het opgegeven ID moet een positief geheel getal zijn.", nameof(movieId));
            }                

            Movie movieToUpdate = await _appDBContext.Movies.FirstOrDefaultAsync(m => m.Id == movieId);

            if (movieToUpdate == null)
            {
                throw new InvalidOperationException("Film bestaat niet in de database.");
            }               

            movieToUpdate.Seen = seen;

            _appDBContext.Movies.Update(movieToUpdate);
            await _appDBContext.SaveChangesAsync();
        }

        // Gemaakt om later mogelijke uitbreiding te doen. Zal nu niet gebruikt worden.
        /// <summary>
        /// Stelt de 'Seen'-status in voor een film op basis van een (gedeeltelijke) titel.
        /// </summary>
        /// <param name="partialTitle">Een (gedeeltelijke) titel van de film.</param>
        /// <param name="seen">De nieuwe waarde voor de 'Seen'-status (true = gezien, false = niet gezien).</param>
        /// <exception cref="ArgumentException">Wanneer de titel leeg of ongeldig is.</exception>
        /// <exception cref="InvalidOperationException">Wanneer geen film met de opgegeven (deel)titel wordt gevonden.</exception>
        public async Task SetMovieSeenStatusByTitleAsync(string partialTitle, bool seen)
        {
            if (string.IsNullOrWhiteSpace(partialTitle))
                throw new ArgumentException("Titel mag niet leeg zijn.", nameof(partialTitle));

            Movie movie = await GetMovieByPartialTitleAsync(partialTitle);
            if (movie == null)
                throw new InvalidOperationException("Geen film gevonden met de opgegeven titel.");

            movie.Seen = seen;

            _appDBContext.Movies.Update(movie);
            await _appDBContext.SaveChangesAsync();
        }


        #endregion

        #endregion

        #region Methods to avoid duplicat code

        /// <summary>
        /// Converteert een lijst van namen (opgeslagen in een dictionary) naar een lijst van entiteiten van type T.  
        /// Dit wordt gedaan door een nieuwe instantie van T te maken en de eigenschap "Name" in te vullen met de gevonden waarden.  
        /// </summary>
        /// <typeparam name="T">Het type entiteit dat wordt gemaakt. Moet een parameterloze constructor hebben.</typeparam>
        /// <param name="data">Een dictionary waarin de sleutel een categorie is en de waarde een lijst van namen.</param>
        /// <param name="key">De sleutel in de dictionary waarvan de lijst van namen moet worden omgezet naar entiteiten.</param>
        /// <returns>Een lijst met objecten van type T, waarbij de eigenschap "Name" is ingevuld met de waarden uit de dictionary.</returns>

        public List<T> ConvertNamesToEntities<T>(Dictionary<string, List<string>> data, string key) where T : new()
        {
            List<T> entities = new List<T>(); // Initialiseer een lege lijst om de geconverteerde objecten op te slaan.

            if (data.ContainsKey(key)) // Controleer of de dictionary de opgegeven sleutel bevat.
            {
                foreach (string name in data[key]) // Itereer over de lijst met namen die aan de sleutel gekoppeld zijn.
                {
                    T entity = new T(); // Maak een nieuwe instantie van T. Dit werkt alleen omdat T een parameterloze constructor moet hebben.

                    var property = typeof(T).GetProperty("Name"); // Zoek de "Name"-eigenschap van het type T.

                    if (property != null) // Controleer of de eigenschap "Name" bestaat.
                    {
                        property.SetValue(entity, name); // Stel de waarde van de "Name"-eigenschap in op de huidige naam uit de lijst.
                    }

                    entities.Add(entity); // Voeg de nieuw gemaakte entiteit toe aan de lijst.
                }
            }

            return entities; // Retourneer de lijst met geconverteerde entiteiten.
        }

        /// <summary>
        /// Zoekt een entiteit op basis van een gedeeltelijke string-match in de opgegeven DbSet.
        /// Indien er meerdere matches zijn, krijgt de gebruiker de mogelijkheid om een specifieke entiteit te kiezen.
        /// </summary>
        /// <typeparam name="T">Het type entiteit dat wordt opgehaald.</typeparam>
        /// <param name="dbSetSelector">Een functie die de juiste DbSet uit de databasecontext selecteert.</param>
        /// <param name="propertySelector">Een functie die bepaalt welke eigenschap van de entiteit wordt gebruikt voor de vergelijking.</param>
        /// <param name="partialValue">De zoekterm die wordt gebruikt om een gedeeltelijke match te vinden.</param>
        /// <returns>De gevonden entiteit of null als er geen overeenkomsten zijn.</returns>

        public async Task<T> GetByPartialMatchAsync<T>(
            Func<AppDBContext, DbSet<T>> dbSetSelector, // Functie die bepaalt welke DbSet wordt gebruikt
            Func<T, string> propertySelector, // Functie die bepaalt welk property van het object wordt vergeleken
            string partialValue) where T : class // De zoekterm die wordt gebruikt voor filtering
        {
            if (string.IsNullOrWhiteSpace(partialValue)) // Controleert of de zoekterm leeg is
            {
                throw new ArgumentException("De waarde mag niet leeg zijn.", nameof(partialValue));
            }

            // dbSetSelector is een parameter die een functie (Func<AppDBContext, DbSet<T>>) verwacht. => (GetByPartialMatchAsync) 

            DbSet<T> dbSet = dbSetSelector(_appDBContext); // Ophalen van de juiste DbSet via de selectorfunctie (GetByPartialMatchAsync) 

            // Stap 1: Eerst alle entiteiten ophalen uit de database
            List<T> allEntities = await dbSet.ToListAsync();

            // Stap 2: Filteren in geheugen met StringComparison.OrdinalIgnoreCase (case-insensitive zoekopdracht)
            List<T> entities = allEntities
                .Where(entity => propertySelector(entity).Contains(partialValue, StringComparison.OrdinalIgnoreCase)) // Hiermee kan ik een hoofdletter-ongevoelige vergelijking uitvoeren. bijvoorbeel movie en MOvie worden als gelijke beschouwd. Ordinal houd rekening met de numerieke waarde van een teken (unicode)
                .ToList();

            if (!entities.Any()) return null; // Geen resultaten gevonden, return null
            if (entities.Count == 1) return entities.First(); // Exact één resultaat, return dat object

            // Stap 3: Gebruiker een keuze laten maken als er meerdere resultaten zijn
            return await SelectEntityByIdAsync(entities, propertySelector);
        }

        /// <summary>
        /// Laat de gebruiker een entiteit kiezen uit een lijst op basis van ID.
        /// De gebruiker kan een geldig ID invoeren, 'terug' typen om opnieuw te zoeken, of 'exit' om het programma af te sluiten.
        /// </summary>
        /// <typeparam name="T">Het type entiteit in de lijst.</typeparam>
        /// <param name="entities">De lijst met entiteiten waaruit de gebruiker kan kiezen.</param>
        /// <param name="propertySelector">Een functie die de naam of beschrijving van de entiteit bepaalt.</param>
        /// <returns>De geselecteerde entiteit of null als de gebruiker teruggaat.</returns>

        private async Task<T> SelectEntityByIdAsync<T>(List<T> entities, Func<T, string> propertySelector) where T : class
        {
            if (entities == null || !entities.Any()) // Controleert of de lijst leeg is
            {
                Console.WriteLine("Geen resultaten gevonden. Probeer opnieuw met een andere zoekopdracht.");
                return null; // Teruggaan om opnieuw te zoeken
            }

            PropertyInfo idProperty = typeof(T).GetProperty("Id"); // Ophalen van de 'Id' property van het object
            if (idProperty == null)
            {
                throw new InvalidOperationException("De entiteit moet een 'Id' eigenschap hebben.");
            }

            while (true) // Blijft in de lus totdat de gebruiker een geldige ID invoert of teruggaat
            {
                Console.WriteLine("\nMeerdere resultaten gevonden. Kies een ID, typ 'terug' om opnieuw te zoeken, of 'exit' om te stoppen:");

                foreach (T entity in entities) // Toon alle resultaten met ID
                {
                    int id = (int)idProperty.GetValue(entity); // Haal de waarde van de 'Id' property op
                    Console.WriteLine($"{id}: {propertySelector(entity)}"); // Toon ID en naam/titel
                }

                Console.Write("\nVoer het ID van de gewenste entiteit in ('terug' om opnieuw te zoeken, 'exit' om te stoppen): ");
                string input = Console.ReadLine().Trim(); // Lees gebruikersinput

                if (input.Equals("exit", StringComparison.OrdinalIgnoreCase)) // Sluit het programma af als de gebruiker 'exit' invoert
                {
                    Console.WriteLine("Programma wordt afgesloten...");
                    Environment.Exit(0);
                }

                if (input.Equals("terug", StringComparison.OrdinalIgnoreCase)) // Geeft de gebruiker de mogelijkheid om terug te gaan
                {
                    Console.WriteLine("Terug naar de vorige stap...");
                    return null; // Terug zonder selectie
                }

                if (int.TryParse(input, out int selectedId)) // Controleert of de input een geldig nummer is
                {
                    T selectedEntity = entities.FirstOrDefault(entity => (int)idProperty.GetValue(entity) == selectedId); // Zoek de entiteit op basis van ID

                    if (selectedEntity != null)
                    {
                        return selectedEntity; // Return het geselecteerde object
                    }

                    Console.WriteLine("ID niet gevonden. Probeer opnieuw."); // Geef een melding als het ID niet gevonden is
                }
                else
                {
                    Console.WriteLine("Ongeldige invoer. Voer een numerieke ID in, 'terug' om opnieuw te zoeken, of 'exit' om te stoppen.");
                }
            }
        }

        // Methode om een film te zoeken op basis van een deel van de titel
        public async Task<Movie> GetMovieByPartialTitleAsync(string partialTitle)
        {
            return await GetByPartialMatchAsync(
                db => db.Movies, // Geeft de DbSet van films terug => wordt de functie dbSelector
                movie => movie.Title, // Gebruikt de titel van de film als eigenschap om te filteren
                partialTitle // De zoekterm
            );
        }

        // Methode om een acteur te zoeken op basis van een deel van de naam
        public async Task<Actor> GetActorByPartialNameAsync(string partialName)
        {
            return await GetByPartialMatchAsync(
                db => db.Actors, // Geeft de DbSet van acteurs terug => wordt de functie dbSelector
                actor => actor.Name, // Gebruikt de naam van de acteur als eigenschap om te filteren
                partialName // De zoekterm
            );
        }

        // Methode om een regisseur te zoeken op basis van een deel van de naam
        public async Task<Director> GetDirectorByPartialNameAsync(string partialName)
        {
            return await GetByPartialMatchAsync(
                db => db.Directors, // Geeft de DbSet van regisseurs terug => wordt de functie dbSelector
                director => director.Name, // Gebruikt de naam van de regisseur als eigenschap om te filteren
                partialName // De zoekterm
            );
        }

        // Methode om na te gaan of een jaartal enkel maar bestaat uit cijfers
        public bool CheckForCharacters(string strYear)
        {
            char[] chars = strYear.ToCharArray();

            foreach (char c in chars)
            {
                if (!char.IsDigit(c) || c.Equals("-"))
                {
                    return false;
                }
            }

            return true;
        }
        #endregion

    }

}
