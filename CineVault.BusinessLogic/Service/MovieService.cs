using System;
using System.Collections.Generic;
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

        //// Toegevoegde code om films, cast en crew op te slaan

        private readonly AppDBContext _appDBContext;

        #endregion

        #region Properties

        public List<Movie>? MoviesFromApi { get; set; }
        public List<int>? ListWithReceivedIds { get; set; }

        #endregion



        #region Constructor

        public MovieService(IMovieRepository movieRepository, ApiService apiService, AppDBContext appDbContext)
        {
            _movieRepository = movieRepository;
            _apiService = apiService;
            _appDBContext = appDbContext;
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

        #region looking for method 1

        //public void AddMovieAndDetailsByTitle(string movieTitle, int selectedMovieIMDBId)
        //{
        //    if (string.IsNullOrEmpty(movieTitle))
        //    {
        //        throw new ArgumentNullException(nameof(movieTitle), "Movie title cannot be NULL or empty.");
        //    }

        //    // Stap 1: Zoek films via ApiService
        //    List<Movie>? moviesFromApi = _apiService.GetMoviesByTitle(movieTitle).Result;

        //    if (moviesFromApi == null || !moviesFromApi.Any())
        //    {
        //        throw new InvalidOperationException("Geen films gevonden met de opgegeven titel.");
        //    }

        //    // Stap 2: Controleer of de geselecteerde film aanwezig is in de lijst van opgehaalde films
        //    Movie? selectedMovie = moviesFromApi.FirstOrDefault(m => m.IMDBId == selectedMovieIMDBId);
        //    if (selectedMovie == null)
        //    {
        //        throw new InvalidOperationException("De geselecteerde film is niet gevonden in de resultaten van de API.");
        //    }

        //    // Stap 3: Controleer of de film al in de database bestaat
        //    if (_movieRepository.CheckIfMovieExists(selectedMovie.Title, selectedMovie.Year))
        //    {
        //        throw new InvalidOperationException("Een film met dezelfde titel en releasejaar bestaat al.");
        //    }

        //    // Voeg de film toe aan de database
        //    _movieRepository.AddMovieByMovie(selectedMovie);

        //    // Stap 4: Haal acteurs en regisseurs van de geselecteerde film op
        //    Dictionary<string, List<string>>? castAndCrew = _apiService.GetActorsAndDirectorsFromMovieId(selectedMovie.IMDBId).Result;
        //    if (castAndCrew == null)
        //    {
        //        throw new InvalidOperationException("Geen gegevens opgehaald van de API.");
        //    }

        //    foreach (KeyValuePair<string, List<string>> entry in castAndCrew)
        //    {
        //        if (entry.Value.Contains("actor"))
        //        {
        //            // Controleer of de acteur al bestaat in de database
        //            Actor existingActor = _appDBContext.Actors.FirstOrDefault(a => a.Name == entry.Key);
        //            if (existingActor == null)
        //            {
        //                Actor newActor = new Actor { Name = entry.Key };
        //                _appDBContext.Actors.Add(newActor);
        //            }
        //        }

        //        if (entry.Value.Contains("director"))
        //        {
        //            // Controleer of de regisseur al bestaat in de database
        //            Director existingDirector = _appDBContext.Directors.FirstOrDefault(d => d.Name == entry.Key);
        //            if (existingDirector == null)
        //            {
        //                Director newDirector = new Director { Name = entry.Key };
        //                _appDBContext.Directors.Add(newDirector);
        //            }
        //        }
        //    }

        //    // Sla de wijzigingen van Acteurs en Regisseurs op
        //    _appDBContext.SaveChanges();

        //    // Stap 5: Maak relaties tussen de film, acteurs en regisseurs
        //    Movie movie = _appDBContext.Movies.FirstOrDefault(m => m.IMDBId == selectedMovie.IMDBId);
        //    if (movie == null)
        //    {
        //        throw new InvalidOperationException("Film bestaat niet in de database.");
        //    }

        //    foreach (KeyValuePair<string, List<string>> entry in castAndCrew)
        //    {
        //        if (entry.Value.Contains("actor"))
        //        {
        //            Actor actor = _appDBContext.Actors.FirstOrDefault(a => a.Name == entry.Key);
        //            if (actor != null)
        //            {
        //                MovieActor movieActor = new MovieActor
        //                {
        //                    MovieId = movie.Id,
        //                    ActorId = actor.Id
        //                };
        //                _appDBContext.MovieActors.Add(movieActor);
        //            }
        //        }

        //        if (entry.Value.Contains("director"))
        //        {
        //            Director director = _appDBContext.Directors.FirstOrDefault(d => d.Name == entry.Key);
        //            if (director != null)
        //            {
        //                MovieDirector movieDirector = new MovieDirector
        //                {
        //                    MovieId = movie.Id,
        //                    DirectorId = director.Id
        //                };
        //                _appDBContext.MovieDirectors.Add(movieDirector);
        //            }
        //        }
        //    }

        //    // Sla de relaties op in de database
        //    _appDBContext.SaveChanges();
        //}

        #endregion

        #region Looking for method 2

        // Stap 1: Zoek films op basis van de titel

        //////////public List<Movie> SearchMoviesByTitle(string movieTitle)
        //////////{
        //////////    if (string.IsNullOrEmpty(movieTitle))
        //////////    {
        //////////        throw new ArgumentNullException(nameof(movieTitle), "Movie title cannot be NULL or empty.");
        //////////    }

        //////////    // Zoek films via ApiService
        //////////    List<Movie>? moviesFromApi = _apiService.GetMoviesByTitle(movieTitle).Result;

        //////////    if (moviesFromApi == null || !moviesFromApi.Any())
        //////////    {
        //////////        throw new InvalidOperationException("Geen films gevonden met de opgegeven titel.");
        //////////    }

        //////////    foreach (Movie movie in moviesFromApi)
        //////////    {
        //////////        Console.WriteLine(movie.Title + " " + movie.Year + " " + movie.IMDBId);
        //////////    }

        //////////    // Retourneer de lijst met films
        //////////    return moviesFromApi;
        //////////}

        //////////// Stap 2: Voeg de geselecteerde film toe aan de database
        //////////public void AddSelectedMovieByIMDBId(int selectedMovieIMDBId, List<Movie> moviesFromApi)
        //////////{
        //////////    if (moviesFromApi == null || !moviesFromApi.Any())
        //////////    {
        //////////        throw new ArgumentNullException(nameof(moviesFromApi), "De lijst met films mag niet leeg zijn.");
        //////////    }

        //////////    // Controleer of de geselecteerde film aanwezig is in de lijst van opgehaalde films
        //////////    Movie? selectedMovie = moviesFromApi.FirstOrDefault(m => m.IMDBId == selectedMovieIMDBId);
        //////////    if (selectedMovie == null)
        //////////    {
        //////////        throw new InvalidOperationException("De geselecteerde film is niet gevonden in de resultaten van de API.");
        //////////    }

        //////////    // Controleer of de film al in de database bestaat
        //////////    if (_movieRepository.CheckIfMovieExists(selectedMovie.Title, selectedMovie.Year))
        //////////    {
        //////////        throw new InvalidOperationException("Een film met dezelfde titel en releasejaar bestaat al.");
        //////////    }

        //////////    // Voeg de film toe aan de database
        //////////    _movieRepository.AddMovieByMovie(selectedMovie);

        //////////    // Sla acteurs en regisseurs van de geselecteerde film op
        //////////    SaveActorsAndDirectorsFromApi(selectedMovie.IMDBId);
        //////////}

        //////////// Opslaan van acteurs en regisseurs in de database
        //////////private void SaveActorsAndDirectorsFromApi(int movieId)
        //////////{
        //////////    Dictionary<string, List<string>>? castAndCrew = _apiService.GetActorsAndDirectorsFromMovieId(movieId).Result;
        //////////    if (castAndCrew == null)
        //////////    {
        //////////        throw new InvalidOperationException("Geen gegevens opgehaald van de API.");
        //////////    }

        //////////    foreach (KeyValuePair<string, List<string>> entry in castAndCrew)
        //////////    {
        //////////        if (entry.Value.Contains("actor"))
        //////////        {
        //////////            Actor existingActor = _appDBContext.Actors.FirstOrDefault(a => a.Name == entry.Key);
        //////////            if (existingActor == null)
        //////////            {
        //////////                Actor newActor = new Actor { Name = entry.Key };
        //////////                _appDBContext.Actors.Add(newActor);
        //////////            }
        //////////        }

        //////////        if (entry.Value.Contains("director"))
        //////////        {
        //////////            Director existingDirector = _appDBContext.Directors.FirstOrDefault(d => d.Name == entry.Key);
        //////////            if (existingDirector == null)
        //////////            {
        //////////                Director newDirector = new Director { Name = entry.Key };
        //////////                _appDBContext.Directors.Add(newDirector);
        //////////            }
        //////////        }
        //////////    }

        //////////    _appDBContext.SaveChanges();

        //////////    Movie movie = _appDBContext.Movies.FirstOrDefault(m => m.IMDBId == movieId);
        //////////    if (movie == null)
        //////////    {
        //////////        throw new InvalidOperationException("Film bestaat niet in de database.");
        //////////    }

        //////////    foreach (KeyValuePair<string, List<string>> entry in castAndCrew)
        //////////    {
        //////////        if (entry.Value.Contains("actor"))
        //////////        {
        //////////            Actor actor = _appDBContext.Actors.FirstOrDefault(a => a.Name == entry.Key);
        //////////            if (actor != null)
        //////////            {
        //////////                MovieActor movieActor = new MovieActor
        //////////                {
        //////////                    MovieId = movie.Id,
        //////////                    ActorId = actor.Id
        //////////                };
        //////////                _appDBContext.MovieActors.Add(movieActor);
        //////////            }
        //////////        }

        //////////        if (entry.Value.Contains("director"))
        //////////        {
        //////////            Director director = _appDBContext.Directors.FirstOrDefault(d => d.Name == entry.Key);
        //////////            if (director != null)
        //////////            {
        //////////                MovieDirector movieDirector = new MovieDirector
        //////////                {
        //////////                    MovieId = movie.Id,
        //////////                    DirectorId = director.Id
        //////////                };
        //////////                _appDBContext.MovieDirectors.Add(movieDirector);
        //////////            }
        //////////        }
        //////////    }

        //////////    _appDBContext.SaveChanges();
        //////////}

        #endregion

        #region looking for method 3

        //public void AddMovieByMovie(string movieTitle, int selectedMovieIMDBId)
        //{
        //    if (string.IsNullOrEmpty(movieTitle))
        //    {
        //        throw new ArgumentNullException(nameof(movieTitle), "Movie title cannot be NULL or empty.");
        //    }

        //    // Stap 1: Zoeken van de film aan de hand van de ApiService.
        //    List<Movie>? moviesFromApi = _apiService.GetMoviesByTitle(movieTitle).Result;
        //    if (moviesFromApi == null || !moviesFromApi.Any())
        //    {
        //        throw new InvalidOperationException("Geen films gevonden met de opgegeven titel.");
        //    }

        //    // Stap 2: Controleren of de geselecteerde film aanwezig is in de lijst van opgehaalde films.
        //    Movie? selectedMovie = moviesFromApi.FirstOrDefault(m => m.IMDBId == selectedMovieIMDBId);
        //    if (selectedMovie == null)
        //    {
        //        throw new InvalidOperationException("De geselecteerde film is niet gevonden in de resultaten van de API.");
        //    }

        //    // Stap 3: Controleer of de film al in de database bestaat.
        //    if (_movieRepository.CheckIfMovieExists(selectedMovie.Title, selectedMovie.Year))
        //    {
        //        throw new InvalidOperationException("Een film met dezelfde titel en releasejaar bestaat al.");
        //    }

        //    // Voegt de film toe aan de database.
        //    _movieRepository.AddMovieByMovie(selectedMovie);

        //    // Stap 4: Sla acteurs en regisseurs van de geselecteerde film op.
        //    SaveActorsAndDirectorsFromApi(selectedMovie.IMDBId);
        //}

        public void AddMovieByMovie(Movie movie)
        {
            _apiService.GetMoviesByTitle(movie.Title);

            if (movie == null)
            {
                throw new ArgumentNullException(nameof(movie), "movie cannot be NULL");
            }

            // ToDo: Deze methode is aangemaakt in de interface en de klasse zelf. Nog controleren of deze methode werkt.
            //////////////////////if (_movieRepository.CheckIfMovieExists(movie.Title, movie.Year))
            //////////////////////{
            //////////////////////    throw new InvalidOperationException("A movie with the same title and release date already exists.");
            //////////////////////}


            _movieRepository.AddMovieByMovie(movie);
        }

        #endregion

        public void AddMovieByTitle(string movieTitle)
        {
            bool blCorrectyEnteredId;
            int intEnteredId;
            int intResultId;
            string strMovieToSearchFor = movieTitle.Trim().ToLower();
            string strEnteredId;
            string strSelectedMovieBasedOnId;
            Movie selectedMovie;

            blCorrectyEnteredId = false;
            intResultId = 0;
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

            foreach (Movie movie in MoviesFromApi)
            {
                Console.WriteLine(movie.Title + " " + movie.Year + ": " + movie.IMDBId);
                ListWithReceivedIds.Add(movie.IMDBId);
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
                    intResultId = intEnteredId;
                    blCorrectyEnteredId = true;
                }

            }

            // Nu wordt de juiste film gekozen uit de MoviesFromAPi-lijst

            for (int index = 0; index < MoviesFromApi.Count; index++)
            {
                if (MoviesFromApi[index].IMDBId.Equals(intResultId))
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

        }

        public void RemoveMovieByMovie(Movie movie)
        {

            if (movie == null)
            {
                throw new ArgumentNullException(nameof(movie), "movie cannot be NULL");
            }

            _movieRepository.RemoveMovieByMovie(movie);
        }

        #endregion

        #region Retrieve movies by status

        public IEnumerable<Movie> ShowAllMoviesAUserContains()
        {
            return _movieRepository.ShowAllMoviesAUserContains()
                .OrderBy(movie => movie.Title);
        }

        public IEnumerable<Movie> ShowAllMoviesThatHaveBeenSeen()
        {
            return _movieRepository.ShowAllMoviesThatHaveBeenSeen()
                .OrderBy(movie => movie.Title);
        }

        public IEnumerable<Movie> ShowAllMoviesThatHaveNotBeenSeen()
        {
            return _movieRepository.ShowAllMoviesThatHaveNotBeenSeen()
                .OrderBy(movie => movie.Title);
        }

        #endregion

        #region Filter by movie data

        public IEnumerable<Actor> ShowAllActorsFromMovie(Movie movie)
        {

            if (movie == null)
            {
                throw new ArgumentNullException(nameof(movie));
            }

            return _movieRepository.ShowAllActorsFromMovie(movie);
        }

        public IEnumerable<Director> ShowDirectorFromMovie(Movie movie)
        {

            if (movie == null)
            {
                throw new ArgumentNullException(nameof(movie));
            }

            return _movieRepository.ShowDirectorFromMovie(movie);
        }

        public string ShowYearFromMovie(Movie movie)
        {

            if (movie == null)
            {
                throw new ArgumentNullException(nameof(movie));
            }

            return _movieRepository.ShowYearFromMovie(movie);
        }

        #endregion

        #region Filter by model

        public IEnumerable<Movie> ShowMoviesFromTheSameActor(Actor actor)
        {

            if (actor == null)
            {
                throw new ArgumentNullException(nameof(actor));
            }

            return _movieRepository.ShowMoviesFromTheSameActor(actor);
        }

        public IEnumerable<Movie> ShowMoviesFromTheSameDirector(Director director)
        {

            if (director == null)
            {
                throw new ArgumentNullException(nameof(director));
            }

            return _movieRepository.ShowMoviesFromTheSameDirector(director);
        }

        public IEnumerable<Movie> ShowAllMoviesFromTheSameYear(string strYear)
        {

            if (string.IsNullOrWhiteSpace(strYear))
            {
                throw new ArgumentException("Year cannot be null or empty", nameof(strYear));
            }

            return _movieRepository.ShowAllMoviesFromTheSameYear(strYear);
        }

        #endregion

        #endregion

        #region Using adapter



        #endregion

        //////public void SaveActorsAndDirectorsFromApi(int movieId)
        //////{
        //////    Dictionary<string, List<string>>? castAndCrew = _apiService.GetActorsAndDirectorsFromMovieId(movieId).Result;
        //////    if (castAndCrew == null)
        //////    {
        //////        throw new InvalidOperationException("Geen gegevens opgehaald van de API.");
        //////    }

        //////    foreach (KeyValuePair<string, List<string>> entry in castAndCrew)
        //////    {
        //////        if (entry.Value.Contains("actor"))
        //////        {
        //////            Actor existingActor = _appDBContext.Actors.FirstOrDefault(a => a.Name == entry.Key);
        //////            if (existingActor == null)
        //////            {
        //////                Actor newActor = new Actor { Name = entry.Key };
        //////                _appDBContext.Actors.Add(newActor);
        //////            }

        //////        }

        //////        if (entry.Value.Contains("director"))
        //////        {
        //////            Director existingDirector = _appDBContext.Directors.FirstOrDefault(d => d.Name == entry.Key);
        //////            if (existingDirector == null)
        //////            {
        //////                Director newDirector = new Director { Name = entry.Key };
        //////                _appDBContext.Directors.Add(newDirector);
        //////            }

        //////        }

        //////    }

        //////    _appDBContext.SaveChanges();

        //////    Movie movie = _appDBContext.Movies.FirstOrDefault(m => m.IMDBId == movieId);

        //////    if (movie == null)
        //////    {
        //////        throw new InvalidOperationException("Film bestaat niet in de database.");
        //////    }

        //////    foreach (KeyValuePair<string, List<string>> entry in castAndCrew)
        //////    {
        //////        if (entry.Value.Contains("actor"))
        //////        {
        //////            Actor actor = _appDBContext.Actors.FirstOrDefault(a => a.Name == entry.Key);
        //////            if (actor != null)
        //////            {
        //////                MovieActor movieActor = new MovieActor
        //////                {
        //////                    MovieId = movie.Id,
        //////                    ActorId = actor.Id
        //////                };

        //////                _appDBContext.MovieActors.Add(movieActor);
        //////            }

        //////        }

        //////        if (entry.Value.Contains("director"))
        //////        {
        //////            Director director = _appDBContext.Directors.FirstOrDefault(d => d.Name == entry.Key);
        //////            if (director != null)
        //////            {
        //////                MovieDirector movieDirector = new MovieDirector
        //////                {
        //////                    MovieId = movie.Id,
        //////                    DirectorId = director.Id
        //////                };

        //////                _appDBContext.MovieDirectors.Add(movieDirector);
        //////            }

        //////        }

        //////    }

        //////    _appDBContext.SaveChanges();
        //////}

    }
}
