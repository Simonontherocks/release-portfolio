using CineVault.BusinessLogic.Service;
using CineVault.ModelLayer.ModelAbstractClass;
using CineVault.ModelLayer.ModelMovie;
using CineVault.ModelLayer.ModelUser;
using CineVault.ModelLayer.ModelLayerService;
using CineVault.DataAccessLayer;
using System.IO;
using System.Reflection;
using CineVault.BusinessLogic.ApiModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace CineVault.MainProgram
{
    internal class Program
    {
        private static AppDBContext _appDBContext;
        static private ApiService _apiService = new ApiService();

        static async Task Main(string[] args)
        {

            ServiceCollection services = new ServiceCollection();

            services.AddDbContext<AppDBContext>(options => options.UseSqlServer("server=SIMON\\SQLEXPRESS;Database=CineVault; Trusted_Connection=true; TrustServerCertificate=True"));

            ServiceProvider serviceProvider = services.BuildServiceProvider();
            _appDBContext = serviceProvider.GetService<AppDBContext>();

            //List<Movie>? movieList;
            //movieList = await apiService.GetMoviesByTitle("braveheart");
            //if (movieList.Count > 0)
            //{
            //    var dict = await apiService.GetActorsAndDirectorsFromMovieId(movieList[0].IMDBId);
            //}      

            //Console.ReadLine();

            Console.WriteLine("1: toon alle films");
            Console.WriteLine("2: voeg film toe");
            int keuze;
            keuze = int.Parse(Console.ReadLine());
            switch (keuze)
            {
                case 1:
                    GetAllMovies();
                    break;
                case 2:
                    await AddMovie();
                    break;
                default:
                    break;
            }

            Console.WriteLine("Program beindigt");
            Console.ReadLine();

        }

        private static async Task AddMovie()
        {
            CineVault.BusinessLogic.Service.MovieService service = new MovieService(new MovieRepository(_appDBContext), new ApiService());
            CineVault.BusinessLogic.Service.ActorService actorService = new ActorService(new ActorRepository(_appDBContext));
            CineVault.BusinessLogic.Service.DirectorService directorService = new DirectorService(new DirectorRepository(_appDBContext));
            List<Actor> listOfActors = actorService.GetAll();
            List<Director> listOfDirectors = directorService.GetAll();
            List<string> listOfActorNames = new List<string>();
            List<string> listOfDirectorNames = new List<string>();
            List<int> listWithImdbNumbers = new List<int>();

            foreach (Actor actor in listOfActors)
            {
                listOfActorNames.Add(actor.Name);
            }

            foreach (Director director in listOfDirectors)
            {
                listOfDirectorNames.Add(director.Name);
            }

            Console.WriteLine("Geef je filmtitel in");
            string titel = Console.ReadLine();

            //ToDo => controleren op NULL
            List<Movie> listOfMovies = await _apiService.GetMoviesByTitle(titel);

            foreach (Movie movie in listOfMovies)
            {
                Console.WriteLine(movie.IMDBId + " - " + movie.Title + " - " + movie.Year);
                listWithImdbNumbers.Add(movie.IMDBId);
            }

            Console.WriteLine("Geef imdbId in");

            int imdb_id = CheckForEnteredId(listWithImdbNumbers);

            Dictionary<string, List<string>>? listOfActorsAndDirectors = await _apiService.GetActorsAndDirectorsFromMovieId(imdb_id);

            // ToDo => controleren op NULL
            foreach (KeyValuePair<string, List<string>> keyValue in listOfActorsAndDirectors) // Hier worden alle api-models omgezet naar de correcte models. Behalve Movie (deze staat correct).
            {
                if (!listOfActorNames.Contains(keyValue.Key)) // Hier wordt gecontroleerd of de lijst met acteurnamen de key bevat.
                {
                    if (keyValue.Value.Contains("actor")) // als het een acteur is.
                    {
                        actorService.Insert(new Actor { Name = keyValue.Key });
                    }

                }
                else
                {
                    // moet de link tussen movie en actor toegevoegd worden,
                    // of de link tussen movie en director toegevoegd worden.
                }
                Console.WriteLine(keyValue.Key);

                if (!listOfActorNames.Contains(keyValue.Key))
                {
                    if (keyValue.Value.Contains("director"))
                    {
                        directorService.Insert(new Director { Name = keyValue.Key });
                    }

                }

            }

        }

        private static void GetAllMovies()
        {
            List<Movie> listOfMovies;

            CineVault.BusinessLogic.Service.MovieService service = new MovieService(new MovieRepository(_appDBContext), new ApiService());
            listOfMovies = service.ShowAllMoviesAUserContains().ToList();
            foreach (Movie movie in listOfMovies)
            {
                Console.WriteLine(movie.Title);
            }

        }

        private static int CheckForEnteredId(List<int> listWithImdbId)
        {
            bool foundedId = false;
            int resultId = 0;

            if (listWithImdbId == null)
            {
                throw new ArgumentNullException("Lijst mag niet leeg zijn");
            }
            else if (listWithImdbId.Count == 0)
            {
                throw new ArgumentException("Er zijn geen IMDB-id's gevonden");
            }
            else
            {
                while (foundedId == false)
                {
                    string strEnteredId = Console.ReadLine();
                    int intEnteredID = int.Parse(strEnteredId);

                    if (strEnteredId.IsNullOrEmpty())
                    {
                        Console.WriteLine("Je moet een ID ingeven dat in de lijst staat");
                    }
                    else if (!listWithImdbId.Contains(intEnteredID))
                    {
                        Console.WriteLine("Het ingegeven id-nummer staat niet in de lijst. \nProbeer opnieuw");
                    }
                    else
                    {
                        foreach (int id in listWithImdbId)
                        {
                            foundedId = true;
                            resultId = intEnteredID;

                        }

                    }

                }

            }

            return resultId;

        }

    }

}