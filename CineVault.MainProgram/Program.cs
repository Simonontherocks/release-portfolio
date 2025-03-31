using CineVault.BusinessLogic.Service;
using CineVault.DataAccessLayer;
using CineVault.DataAccessLayer.Repositories;
using CineVault.ModelLayer.ModelAbstractClass;
using CineVault.ModelLayer.ModelMovie;
using CineVault.ModelLayer.ModelUser;
using System.IO;
using System.Reflection;
using CineVault.BusinessLogic.ApiModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using CineVault.DataAccessLayer.Context;

namespace CineVault.MainProgram
{
    internal class Program
    {
        private static AppDBContext _appDBContext;
        static private ApiService _apiService = new ApiService();

        static async Task Main(string[] args)
        {
            AppDBContext dbContext = new AppDBContext();
            ApiService apiService = new ApiService();
            MovieRepository movieRepository = new MovieRepository(dbContext);
            MovieService movieService = new MovieService(movieRepository, apiService, dbContext);

            #region Testing adding or removing movies

            //movieService.AddMovieByTitle("revenant");
            //movieService.RemoveMovieByIdAsync(27);

            #endregion

            #region Testing retrieve movies by status

            //IEnumerable<Movie> seenMovies = await movieService.ShowAllMoviesThatHaveBeenSeen();
            //PrintEnumerable<Movie>(seenMovies, movie => $"{movie.Title} ({movie.Year})");            
            //Console.WriteLine("----------");
            //IEnumerable<Movie> unseenMovies = await movieService.ShowAllMoviesThatHaveNotBeenSeen();
            //PrintEnumerable<Movie>(unseenMovies, movie => $"{movie.Title} ({movie.Year})");

            #endregion

            #region Filter by movie data

            #endregion

            #region Testing filter by model

            IEnumerable<Movie> moviesFrom2005 = await movieService.ShowAllMoviesFromTheSameYearAsync("2005");
            PrintEnumerable(moviesFrom2005, movie => $"{movie.Title} ({movie.Year})");

            #endregion

            Console.ReadLine();

            // Controleer de database of de film, acteurs en regisseurs zijn opgeslagen.

        }

        // Deze methode dient om alle films te printen die gevraagd worden.
        // Dit is om duplicate code te voorkomen.

        public static void PrintEnumerable<T>(IEnumerable<T> collection, Func<T, string> formatItem)
        {
            foreach (T item in collection)
            {
                Console.WriteLine(formatItem(item));
            }
        }

    }
}
        //////    List<Movie> listOfMovies = new List<Movie>
        //////    {
        //////        new Movie { Id = 1, Title = "Inception", Seen = true, Score = 9, Year = "2010", IMDBId = 0},
        //////        new Movie { Id = 2, Title = "Pulp Fiction", Seen = false, Score = 9, Year = "1994", IMDBId = 0},
        //////        new Movie { Id = 3, Title = "Interstellar", Seen = true, Score = 8.5, Year = "2014", IMDBId = 0},
        //////        new Movie { Id = 4, Title = "Titanic", Seen = true, Score = 8, Year = "1997", IMDBId = 0},
        //////        new Movie { Id = 5, Title = "Sin City", Seen = false, Score = 7, Year = "2005", IMDBId = 0},
        //////        new Movie { Id = 6, Title = "Star Wars", Seen = true, Score = 7.5, Year = "2005", IMDBId = 0}
        //////    };

        //////    List<Actor> listOfActors = new List<Actor>
        //////    {
        //////        new Actor { Id = 1, Name = "Leonardo Dicaprio"},
        //////        new Actor { Id = 2, Name = "Joseph Gordon-Levitt"},
        //////        new Actor { Id = 3, Name = "Elliot Page"},
        //////        new Actor { Id = 4, Name = "John Travolta"},
        //////        new Actor { Id = 5, Name = "Samuel L. Jackson"},
        //////        new Actor { Id = 6, Name = "Matthew McConaughey"},
        //////        new Actor { Id = 7, Name = "Anne Hathaway"},
        //////        new Actor { Id = 8, Name = "Kate Winslet"},
        //////        new Actor { Id = 9, Name = "Quentin Tarantino"}
        //////    };

        //////    List<Director> listOfDirectors = new List<Director>
        //////    {
        //////        new Director { Id = 1, Name = "Christopher Nolan"},
        //////        new Director { Id = 2, Name = "Quentin Tarantino"},
        //////        new Director { Id = 3, Name = "James Cameron"},
        //////        new Director { Id = 4, Name = "Robert Rodriguez"},
        //////        new Director { Id = 5, Name = "George Lucas"}
        //////    };

        //////    List<MovieActor> listOfMovieActors = new List<MovieActor>
        //////    {
        //////        new MovieActor { Id = 1, MovieId = 1, ActorId = 1},
        //////        new MovieActor { Id = 2, MovieId = 1, ActorId = 2},
        //////        new MovieActor { Id = 3, MovieId = 1, ActorId = 3},
        //////        new MovieActor { Id = 4, MovieId = 2, ActorId = 4},
        //////        new MovieActor { Id = 5, MovieId = 2, ActorId = 5},
        //////        new MovieActor { Id = 6, MovieId = 2, ActorId = 9},
        //////        new MovieActor { Id = 7, MovieId = 3, ActorId = 6},
        //////        new MovieActor { Id = 8, MovieId = 3, ActorId = 7},
        //////        new MovieActor { Id = 9, MovieId = 4, ActorId = 1},
        //////        new MovieActor { Id = 10, MovieId = 4, ActorId = 8},
        //////        new MovieActor { Id = 11, MovieId = 5, ActorId = 9},
        //////        new MovieActor { Id = 12, MovieId = 6, ActorId = 5}
        //////    };

        //////    List<MovieDirector> listOfMovieDirectors = new List<MovieDirector>
        //////    {
        //////        new MovieDirector { Id = 1, MovieId = 1, DirectorId = 1},
        //////        new MovieDirector { Id = 2, MovieId = 2, DirectorId = 2},
        //////        new MovieDirector { Id = 3, MovieId = 3, DirectorId = 1},
        //////        new MovieDirector { Id = 4, MovieId = 4, DirectorId = 3},
        //////        new MovieDirector { Id = 5, MovieId = 5, DirectorId = 2},
        //////        new MovieDirector { Id = 6, MovieId = 5, DirectorId = 4},
        //////        new MovieDirector { Id = 7, MovieId = 6, DirectorId = 5}
        //////    };


        //////    ServiceCollection services = new ServiceCollection();

        //////    services.AddDbContext<AppDBContext>(options => options.UseSqlServer("server=SIMON\\SQLEXPRESS;Database=CineVault; Trusted_Connection=true; TrustServerCertificate=True"));

        //////    ServiceProvider serviceProvider = services.BuildServiceProvider();
        //////    _appDBContext = serviceProvider.GetService<AppDBContext>();

        //////    //List<Movie>? movieList;
        //////    //movieList = await apiService.GetMoviesByTitle("braveheart");
        //////    //if (movieList.Count > 0)
        //////    //{
        //////    //    var dict = await apiService.GetActorsAndDirectorsFromMovieId(movieList[0].IMDBId);
        //////    //}      

        //////    //Console.ReadLine();

        //////    Console.WriteLine("1: toon alle films");
        //////    Console.WriteLine("2: voeg film toe");
        //////    int keuze;
        //////    keuze = int.Parse(Console.ReadLine());
        //////    switch (keuze)
        //////    {
        //////        case 1:
        //////            GetAllMovies();
        //////            break;
        //////        case 2:
        //////            await AddMovie();
        //////            break;
        //////        default:
        //////            break;
        //////    }

        //////    Console.WriteLine("Program beindigt");
        //////    Console.ReadLine();

        //////}

        //////private static async Task AddMovie()
        //////{
        //////    CineVault.BusinessLogic.Service.MovieService service = new MovieService(new MovieRepository(_appDBContext), new ApiService());
        //////    CineVault.BusinessLogic.Service.ActorService actorService = new ActorService(new ActorRepository(_appDBContext));
        //////    CineVault.BusinessLogic.Service.DirectorService directorService = new DirectorService(new DirectorRepository(_appDBContext));
        //////    List<Actor> listOfActors = actorService.GetAll();
        //////    List<Director> listOfDirectors = directorService.GetAll();
        //////    List<string> listOfActorNames = new List<string>();
        //////    List<string> listOfDirectorNames = new List<string>();
        //////    List<int> listWithImdbNumbers = new List<int>();

        //////    foreach (Actor actor in listOfActors)
        //////    {
        //////        listOfActorNames.Add(actor.Name);
        //////    }

        //////    foreach (Director director in listOfDirectors)
        //////    {
        //////        listOfDirectorNames.Add(director.Name);
        //////    }

        //////    Console.WriteLine("Geef je filmtitel in");
        //////    string titel = Console.ReadLine();

        //////    //ToDo => controleren op NULL
        //////    List<Movie> listOfMovies = await _apiService.GetMoviesByTitle(titel);

        //////    foreach (Movie movie in listOfMovies)
        //////    {
        //////        Console.WriteLine(movie.IMDBId + " - " + movie.Title + " - " + movie.Year);
        //////        listWithImdbNumbers.Add(movie.IMDBId);
        //////    }

        //////    Console.WriteLine("Geef imdbId in");

        //////    int imdb_id = CheckForEnteredId(listWithImdbNumbers);

        //////    Dictionary<string, List<string>>? listOfActorsAndDirectors = await _apiService.GetActorsAndDirectorsFromMovieId(imdb_id);

        //////    // ToDo => controleren op NULL
        //////    foreach (KeyValuePair<string, List<string>> keyValue in listOfActorsAndDirectors) // Hier worden alle api-models omgezet naar de correcte models. Behalve Movie (deze staat correct).
        //////    {
        //////        if (!listOfActorNames.Contains(keyValue.Key)) // Hier wordt gecontroleerd of de lijst met acteurnamen de key bevat.
        //////        {
        //////            if (keyValue.Value.Contains("actor")) // als het een acteur is.
        //////            {
        //////                actorService.Insert(new Actor { Name = keyValue.Key });
        //////            }

        //////        }
        //////        else
        //////        {
        //////            // moet de link tussen movie en actor toegevoegd worden,
        //////            // of de link tussen movie en director toegevoegd worden.
        //////        }
        //////        Console.WriteLine(keyValue.Key);

        //////        if (!listOfActorNames.Contains(keyValue.Key))
        //////        {
        //////            if (keyValue.Value.Contains("director"))
        //////            {
        //////                directorService.Insert(new Director { Name = keyValue.Key });
        //////            }

        //////        }

        //////    }

        //////}

        //////private static void GetAllMovies()
        //////{
        //////    List<Movie> listOfMovies;

        //////    CineVault.BusinessLogic.Service.MovieService service = new MovieService(new MovieRepository(_appDBContext), new ApiService());
        //////    listOfMovies = service.ShowAllMoviesAUserContains().ToList();
        //////    foreach (Movie movie in listOfMovies)
        //////    {
        //////        Console.WriteLine(movie.Title);
        //////    }

        //////}

        //////private static int CheckForEnteredId(List<int> listWithImdbId)
        //////{
        //////    bool foundedId = false;
        //////    int resultId = 0;

        //////    if (listWithImdbId == null)
        //////    {
        //////        throw new ArgumentNullException("Lijst mag niet leeg zijn");
        //////    }
        //////    else if (listWithImdbId.Count == 0)
        //////    {
        //////        throw new ArgumentException("Er zijn geen IMDB-id's gevonden");
        //////    }
        //////    else
        //////    {
        //////        while (foundedId == false)
        //////        {
        //////            string strEnteredId = Console.ReadLine();
        //////            int intEnteredID = int.Parse(strEnteredId);

        //////            if (strEnteredId.IsNullOrEmpty())
        //////            {
        //////                Console.WriteLine("Je moet een ID ingeven dat in de lijst staat");
        //////            }
        //////            else if (!listWithImdbId.Contains(intEnteredID))
        //////            {
        //////                Console.WriteLine("Het ingegeven id-nummer staat niet in de lijst. \nProbeer opnieuw");
        //////            }
        //////            else
        //////            {
        //////                foreach (int id in listWithImdbId)
        //////                {
        //////                    foundedId = true;
        //////                    resultId = intEnteredID;

        //////                }

        //////            }

        //////        }

        //////    }

        //////    return resultId;

        //////}

    //}

//}