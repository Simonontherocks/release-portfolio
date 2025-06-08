// Is toegevoegd om meteen te kunnen testen, zonder unittests.

using CineVault.BusinessLogic.Service;
using CineVault.DataAccessLayer.Repositories;
using CineVault.DataAccessLayer.Context;

namespace CineVault.MainProgram
{
    //internal class Program
    //{
    //    private static AppDBContext _appDBContext;
    //    private static ApiService _apiService = new ApiService();

    //    static async Task Main(string[] args)
    //    {
    //        AppDBContext dbContext = new AppDBContext();
    //        ApiService apiService = new ApiService();
    //        MovieRepository movieRepository = new MovieRepository(dbContext);
    //        MovieService movieService = new MovieService(movieRepository, apiService, dbContext);

    //        #region Testing adding or removing movies

    //        //movieService.AddMovieByTitle("die hard", false);
    //        //movieService.AddMovieByTitle("armageddon", false);
    //        //movieService.AddMovieByTmdbId(39);
    //        //movieService.RemoveMovieByIdAsync(39);

    //        #endregion

    //        #region Testing retrieve movies by status

    //        //IEnumerable<Movie> seenMovies = await movieService.ShowAllMoviesThatHaveBeenSeen();
    //        //PrintEnumerable<Movie>(seenMovies, movie => $"{movie.Title} ({movie.Year})");            
    //        //Console.WriteLine("----------");
    //        //IEnumerable<Movie> unseenMovies = await movieService.ShowAllMoviesThatHaveNotBeenSeen();
    //        //PrintEnumerable<Movie>(unseenMovies, movie => $"{movie.Title} ({movie.Year})");

    //        #endregion

    //        #region Filter by movie data

    //        //string movieTitle = "jones"; // Nu kan je ook hier een deel van de titel gebruiken
    //        //Movie specificMovie = await movieService.GetMovieByPartialTitleAsync(movieTitle);

    //        //if (specificMovie != null)
    //        //{
    //        //    Console.WriteLine($"Gevonden film: {specificMovie.Title} ({specificMovie.Year})");

    //        //    // Getest => works
    //        //    //// Test acteurs van de film
    //        //    //IEnumerable<Actor> actors = await movieService.ShowAllActorsFromMovieAsync(specificMovie.Title);
    //        //    //PrintEnumerable(actors, actor => actor.Name);

    //        //    // Getest => works
    //        //    //// Test regisseur(s) van de film
    //        //    //IEnumerable<Director> directors = await movieService.ShowDirectorFromMovieAsync(specificMovie.Title);
    //        //    //PrintEnumerable(directors, director => $"Regisseur: {director.Name}");

    //        //    // Getest => works
    //        //    //// Test jaar van de film
    //        //    //string year = await movieService.ShowYearFromMovieAsync(specificMovie.Title);
    //        //    //Console.WriteLine($"Jaar van de film: {year}");
    //        //}
    //        //else
    //        //{
    //        //    Console.WriteLine($"Geen film gevonden met de titel '{movieTitle}'.");
    //        //}

    //        #endregion

    //        #region Testing filter by model

    //        // Getest => works
    //        //string actorName = "ot"; // Je hoeft nu niet de volledige naam in te voeren
    //        //Actor specificActor = await movieService.GetActorByPartialNameAsync(actorName);

    //        //if (specificActor != null)
    //        //{
    //        //    Console.WriteLine($"Gevonden acteur: {specificActor.Name}");

    //        //    // Test films met deze acteur
    //        //    IEnumerable<Movie> movies = await movieService.ShowMoviesFromTheSameActorAsync(specificActor);
    //        //    PrintEnumerable(movies, movie => $"Film met {specificActor.Name}: {movie.Title} ({movie.Year})");
    //        //}
    //        //else
    //        //{
    //        //    Console.WriteLine($"Geen acteur gevonden met de naam '{actorName}'.");
    //        //}

    //        //Getest => works
    //        //string directorName = "Nol"; // Ook hier kan een deel van de naam worden ingevoerd
    //        //Director specificDirector = await movieService.GetDirectorByPartialNameAsync(directorName);

    //        //if (specificDirector != null)
    //        //{
    //        //    Console.WriteLine($"Gevonden regisseur: {specificDirector.Name}");

    //        //    // Test films van deze regisseur
    //        //    IEnumerable<Movie> movies = await movieService.ShowMoviesFromTheSameDirectorAsync(specificDirector);
    //        //    PrintEnumerable(movies, movie => $"Film geregisseerd door {specificDirector.Name}: {movie.Title} ({movie.Year})");
    //        //}
    //        //else
    //        //{
    //        //    Console.WriteLine($"Geen regisseur gevonden met de naam '{directorName}'.");
    //        //}

    //        //IEnumerable<Movie> moviesFrom2005 = await movieService.ShowAllMoviesFromTheSameYearAsync("200");
    //        //PrintEnumerable(moviesFrom2005, movie => $"{movie.Title} ({movie.Year})");

    //        #endregion

    //        Console.ReadLine();

    //    }

    //    // Deze methode dient om alle films te printen die gevraagd worden.
    //    // Dit is om duplicate code te voorkomen.

    //    public static void PrintEnumerable<T>(IEnumerable<T> collection, Func<T, string> formatItem)
    //    {
    //        foreach (T item in collection)
    //        {
    //            Console.WriteLine(formatItem(item));
    //        }
    //    }

    //}
}