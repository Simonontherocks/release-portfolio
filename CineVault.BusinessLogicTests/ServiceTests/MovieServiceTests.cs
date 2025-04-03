using CineVault.BusinessLogic.Service;
using CineVault.DataAccessLayer.Context;
using CineVault.DataAccessLayer.Repositories;
using CineVault.ModelLayer.ModelMovie;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace CineVault.BusinessLogicTests;

[TestClass]
public class MovieServiceTests
{
    #region Field

    private DbContextOptions<AppDBContext> _dbContextOptions;

    #endregion

    #region TestInitialize

    public void Setup()
    {
        // Nieuwe databasecontext per test
        _dbContextOptions = new DbContextOptionsBuilder<AppDBContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Unieke database per test
            .Options;
    }

    #endregion

    #region testen op "Adding or removing movies"

    [TestMethod]
    public async Task AddMovieByTitle_Should_AddMovie_When_ValidTitleProvided()
    {
        // ARRANGE  
        Setup();
        AppDBContext context = new AppDBContext(_dbContextOptions);
        MovieRepository movieRepository = new MovieRepository(context);
        ApiService apiService = new ApiService();
        MovieService movieService = new MovieService(movieRepository, apiService, context);

        string movieTitle = "Inception";
        bool seenStatus = true;

        // ACT  
        await movieService.AddMovieByTitle(movieTitle, seenStatus);

        // ASSERT  
        Movie? addedMovie = context.Movies.FirstOrDefault(m => m.Title == movieTitle);
        Assert.IsNotNull(addedMovie);
        Assert.AreEqual(movieTitle, addedMovie.Title);
        Assert.AreEqual(seenStatus, addedMovie.Seen);
    }

    [TestMethod]
    public async Task AddMovieByTitle_Should_ThrowException_When_MovieAlreadyExists()
    {
        // ARRANGE  
        Setup();
        AppDBContext context = new AppDBContext(_dbContextOptions);
        MovieRepository movieRepository = new MovieRepository(context);
        ApiService apiService = new ApiService();
        MovieService movieService = new MovieService(movieRepository, apiService, context);

        string movieTitle = "Inception";
        bool seenStatus = true;
        Movie existingMovie = new Movie { IMDBId = 12345, Title = movieTitle, Seen = seenStatus };

        // Voeg de film toe
        context.Movies.Add(existingMovie);
        await context.SaveChangesAsync();

        // ACT & ASSERT: Verwacht een uitzondering  
        await Assert.ThrowsExceptionAsync<InvalidOperationException>(async () =>
        {
            await movieService.AddMovieByTitle(movieTitle, seenStatus);
        });
    }

    [TestMethod]
    public async Task AddMovieByTitle_Should_ThrowException_When_MovieNotFound()
    {
        // ARRANGE  
        Setup();
        AppDBContext context = new AppDBContext(_dbContextOptions);
        MovieRepository movieRepository = new MovieRepository(context);
        ApiService apiService = new ApiService(); // Simuleert een lege API response
        MovieService movieService = new MovieService(movieRepository, apiService, context);

        string nonExistingTitle = "Onbestaande Film";
        bool seenStatus = false;

        // ACT & ASSERT: Verwacht een uitzondering  
        await Assert.ThrowsExceptionAsync<InvalidOperationException>(async () =>
        {
            await movieService.AddMovieByTitle(nonExistingTitle, seenStatus);
        });
    }

    [TestMethod]
    public async Task AddMovieByTitle_Should_ThrowException_When_TitleIsEmpty()
    {
        // ARRANGE  
        Setup();
        AppDBContext context = new AppDBContext(_dbContextOptions);
        MovieRepository movieRepository = new MovieRepository(context);
        ApiService apiService = new ApiService();
        MovieService movieService = new MovieService(movieRepository, apiService, context);

        bool seenStatus = false;

        // ACT & ASSERT: Verwacht een fout bij lege titel  
        await Assert.ThrowsExceptionAsync<ArgumentNullException>(async () =>
        {
            await movieService.AddMovieByTitle("", seenStatus);
        });
    }

    [TestMethod]
    public async Task AddMovieByTitle_Should_ThrowException_When_TitleIsNull()
    {
        // ARRANGE  
        Setup();
        AppDBContext context = new AppDBContext(_dbContextOptions);
        MovieRepository movieRepository = new MovieRepository(context);
        ApiService apiService = new ApiService();
        MovieService movieService = new MovieService(movieRepository, apiService, context);

        bool seenStatus = false;

        // ACT
        Func<Task> action = async () => await movieService.AddMovieByTitle(null, seenStatus);

        // ASSERT
        await Assert.ThrowsAsync<ArgumentException>(action);

    }

    [TestMethod]
    public async Task RemoveMovieByIdAsync_Should_RemoveMovie_When_MovieExists()
    {
        // ARRANGE  
        Setup();
        AppDBContext context = new AppDBContext(_dbContextOptions);
        MovieRepository movieRepository = new MovieRepository(context);
        MovieService movieService = new MovieService(movieRepository, null, context);

        Movie movie = new Movie { Id = 1, IMDBId = 12345, Title = "Inception", Seen = true };
        context.Movies.Add(movie);
        await context.SaveChangesAsync();

        // ACT  
        await movieService.RemoveMovieByIdAsync(movie.Id);

        // ASSERT: Film moet verwijderd zijn  
        Movie? removedMovie = context.Movies.FirstOrDefault(m => m.Id == 1);
        Assert.IsNull(removedMovie);
    }

    [TestMethod]
    public async Task RemoveMovieByIdAsync_Should_ThrowException_When_MovieDoesNotExist()
    {
        // ARRANGE  
        Setup();
        AppDBContext context = new AppDBContext(_dbContextOptions);
        MovieRepository movieRepository = new MovieRepository(context);
        MovieService movieService = new MovieService(movieRepository, null, context);

        int nonExistingMovieId = 99999;

        // ACT & ASSERT: Verwacht een uitzondering  
        await Assert.ThrowsExceptionAsync<InvalidOperationException>(async () =>
        {
            await movieService.RemoveMovieByIdAsync(nonExistingMovieId);
        });
    }

    [TestMethod]
    public async Task RemoveMovieByIdAsync_Should_ThrowException_When_DatabaseIsEmpty()
    {
        // ARRANGE  
        Setup();
        AppDBContext context = new AppDBContext(_dbContextOptions);
        MovieRepository movieRepository = new MovieRepository(context);
        MovieService movieService = new MovieService(movieRepository, null, context);

        int movieId = 12345; // Er zit nog geen film in de database

        // ACT & ASSERT: Verwacht een uitzondering  
        await Assert.ThrowsExceptionAsync<InvalidOperationException>(async () =>
        {
            await movieService.RemoveMovieByIdAsync(movieId);
        });
    }

    #endregion

    #region testen op "Retrieve movies by status"

    [TestMethod]
    public async Task ShowAllMovies_Should_NotReturnNull()
    {
        // ARRANGE  
        Setup();
        AppDBContext context = new AppDBContext(_dbContextOptions);
        MovieRepository movieRepository = new MovieRepository(context);
        MovieService movieService = new MovieService(movieRepository, null, context);

        Movie movie1 = new Movie { IMDBId = 1, Title = "Inception", Seen = true };
        Movie movie2 = new Movie { IMDBId = 2, Title = "Interstellar", Seen = false };

        context.Movies.AddRange(movie1, movie2);
        await context.SaveChangesAsync();

        // ACT  
        IEnumerable<Movie> movies = await movieService.ShowAllMovies();

        // ASSERT  
        Assert.IsNotNull(movies);
    }

    [TestMethod]
    public async Task ShowAllMovies_Should_ReturnCorrectMovieCount()
    {
        // ARRANGE  
        Setup();
        AppDBContext context = new AppDBContext(_dbContextOptions);
        MovieRepository movieRepository = new MovieRepository(context);
        MovieService movieService = new MovieService(movieRepository, null, context);

        Movie movie1 = new Movie { IMDBId = 1, Title = "Inception", Seen = true };
        Movie movie2 = new Movie { IMDBId = 2, Title = "Interstellar", Seen = false };

        context.Movies.AddRange(movie1, movie2);
        await context.SaveChangesAsync();

        // ACT  
        IEnumerable<Movie> movies = await movieService.ShowAllMovies();

        // ASSERT  
        Assert.AreEqual(2, movies.Count());
    }

    [TestMethod]
    public async Task ShowAllMoviesThatHaveBeenSeen_Should_NotReturnNull()
    {
        // ARRANGE  
        Setup();
        AppDBContext context = new AppDBContext(_dbContextOptions);
        MovieRepository movieRepository = new MovieRepository(context);
        MovieService movieService = new MovieService(movieRepository, null, context);

        Movie movie1 = new Movie { IMDBId = 1, Title = "Inception", Seen = true };
        Movie movie2 = new Movie { IMDBId = 2, Title = "Interstellar", Seen = false };

        context.Movies.AddRange(movie1, movie2);
        await context.SaveChangesAsync();

        // ACT  
        IEnumerable<Movie> movies = await movieService.ShowAllMoviesThatHaveBeenSeen();

        // ASSERT  
        Assert.IsNotNull(movies);
    }

    [TestMethod]
    public async Task ShowAllMoviesThatHaveBeenSeen_Should_ReturnOnlySeenMovies()
    {
        // ARRANGE  
        Setup();
        AppDBContext context = new AppDBContext(_dbContextOptions);
        MovieRepository movieRepository = new MovieRepository(context);
        MovieService movieService = new MovieService(movieRepository, null, context);

        Movie movie1 = new Movie { IMDBId = 1, Title = "Inception", Seen = true };
        Movie movie2 = new Movie { IMDBId = 2, Title = "Interstellar", Seen = false };

        context.Movies.AddRange(movie1, movie2);
        await context.SaveChangesAsync();

        // ACT  
        IEnumerable<Movie> movies = await movieService.ShowAllMoviesThatHaveBeenSeen();

        // ASSERT  
        Assert.IsTrue(movies.All(m => m.Seen));
    }

    [TestMethod]
    public async Task ShowAllMoviesThatHaveBeenSeen_Should_ReturnCorrectSeenMovieCount()
    {
        // ARRANGE  
        Setup();
        AppDBContext context = new AppDBContext(_dbContextOptions);
        MovieRepository movieRepository = new MovieRepository(context);
        MovieService movieService = new MovieService(movieRepository, null, context);

        Movie movie1 = new Movie { IMDBId = 1, Title = "Inception", Seen = true };
        Movie movie2 = new Movie { IMDBId = 2, Title = "Interstellar", Seen = false };

        context.Movies.AddRange(movie1, movie2);
        await context.SaveChangesAsync();

        // ACT  
        IEnumerable<Movie> movies = await movieService.ShowAllMoviesThatHaveBeenSeen();

        // ASSERT  
        Assert.AreEqual(1, movies.Count());
    }

    [TestMethod]
    public async Task ShowAllMoviesThatHaveNotBeenSeen_Should_NotReturnNull()
    {
        // ARRANGE  
        Setup();
        AppDBContext context = new AppDBContext(_dbContextOptions);
        MovieRepository movieRepository = new MovieRepository(context);
        MovieService movieService = new MovieService(movieRepository, null, context);

        Movie movie1 = new Movie { IMDBId = 1, Title = "Inception", Seen = false };
        Movie movie2 = new Movie { IMDBId = 2, Title = "Interstellar", Seen = true };

        context.Movies.AddRange(movie1, movie2);
        await context.SaveChangesAsync();

        // ACT  
        IEnumerable<Movie> movies = await movieService.ShowAllMoviesThatHaveNotBeenSeen();

        // ASSERT  
        Assert.IsNotNull(movies);
    }

    [TestMethod]
    public async Task ShowAllMoviesThatHaveNotBeenSeen_Should_ReturnOnlyUnseenMovies()
    {
        // ARRANGE  
        Setup();
        AppDBContext context = new AppDBContext(_dbContextOptions);
        MovieRepository movieRepository = new MovieRepository(context);
        MovieService movieService = new MovieService(movieRepository, null, context);

        Movie movie1 = new Movie { IMDBId = 1, Title = "Inception", Seen = false };
        Movie movie2 = new Movie { IMDBId = 2, Title = "Interstellar", Seen = true };

        context.Movies.AddRange(movie1, movie2);
        await context.SaveChangesAsync();

        // ACT  
        IEnumerable<Movie> movies = await movieService.ShowAllMoviesThatHaveNotBeenSeen();

        // ASSERT  
        Assert.IsTrue(movies.All(m => !m.Seen));
    }

    [TestMethod]
    public async Task ShowAllMoviesThatHaveNotBeenSeen_Should_ReturnCorrectUnseenMovieCount()
    {
        // ARRANGE  
        Setup();
        AppDBContext context = new AppDBContext(_dbContextOptions);
        MovieRepository movieRepository = new MovieRepository(context);
        MovieService movieService = new MovieService(movieRepository, null, context);

        Movie movie1 = new Movie { IMDBId = 1, Title = "Inception", Seen = false };
        Movie movie2 = new Movie { IMDBId = 2, Title = "Interstellar", Seen = true };

        context.Movies.AddRange(movie1, movie2);
        await context.SaveChangesAsync();

        // ACT  
        IEnumerable<Movie> movies = await movieService.ShowAllMoviesThatHaveNotBeenSeen();

        // ASSERT  
        Assert.AreEqual(1, movies.Count());
    }

    #endregion

    #region testen op "Filter by movie data"

    [TestMethod]
    public async Task ShowAllActorsFromMovieAsync_Should_NotReturnNull()
    {
        // ARRANGE  
        Setup();
        AppDBContext context = new AppDBContext(_dbContextOptions);
        MovieRepository movieRepository = new MovieRepository(context);
        MovieService movieService = new MovieService(movieRepository, null, context);

        Movie movie = new Movie
        {
            IMDBId = 1,
            Title = "Inception",
            MovieActors = new List<MovieActor>
        {
            new MovieActor { Actor = new Actor { Name = "Leonardo DiCaprio" } },
            new MovieActor { Actor = new Actor { Name = "Joseph Gordon-Levitt" } }
        }
        };

        context.Movies.Add(movie);
        await context.SaveChangesAsync();

        // ACT
        IEnumerable<Actor> actors = await movieService.ShowAllActorsFromMovieAsync("Inception");

        // ASSERT
        Assert.IsNotNull(actors);
    }

    [TestMethod]
    public async Task ShowAllActorsFromMovieAsync_Should_ReturnCorrectActorCount()
    {
        // ARRANGE  
        Setup();
        AppDBContext context = new AppDBContext(_dbContextOptions);
        MovieRepository movieRepository = new MovieRepository(context);
        MovieService movieService = new MovieService(movieRepository, null, context);

        Movie movie = new Movie
        {
            IMDBId = 1,
            Title = "Inception",
            MovieActors = new List<MovieActor>
        {
            new MovieActor { Actor = new Actor { Name = "Leonardo DiCaprio" } },
            new MovieActor { Actor = new Actor { Name = "Joseph Gordon-Levitt" } }
        }
        };

        context.Movies.Add(movie);
        await context.SaveChangesAsync();

        // ACT
        IEnumerable<Actor> actors = await movieService.ShowAllActorsFromMovieAsync("Inception");

        // ASSERT
        Assert.AreEqual(2, actors.Count());
    }

    [TestMethod]
    public async Task ShowAllActorsFromMovieAsync_Should_ThrowException_WhenMovieNotFound()
    {
        // ARRANGE  
        Setup();
        AppDBContext context = new AppDBContext(_dbContextOptions);
        MovieRepository movieRepository = new MovieRepository(context);
        MovieService movieService = new MovieService(movieRepository, null, context);

        // ACT & ASSERT
        await Assert.ThrowsExceptionAsync<InvalidOperationException>(async () =>
        {
            await movieService.ShowAllActorsFromMovieAsync("Niet Bestaande Film");
        });
    }

    [TestMethod]
    public async Task ShowDirectorFromMovieAsync_Should_NotReturnNull()
    {
        // ARRANGE  
        Setup();
        AppDBContext context = new AppDBContext(_dbContextOptions);
        MovieRepository movieRepository = new MovieRepository(context);
        MovieService movieService = new MovieService(movieRepository, null, context);

        Movie movie = new Movie
        {
            IMDBId = 1,
            Title = "Inception",
            MovieDirectors = new List<MovieDirector>
        {
            new MovieDirector
            {
                Director = new Director { Name = "Christopher Nolan" }
            }
        }
        };

        context.Movies.Add(movie);
        await context.SaveChangesAsync();

        // ACT
        IEnumerable<Director> directors = await movieService.ShowDirectorFromMovieAsync("Inception");

        // ASSERT
        Assert.IsNotNull(directors);
    }

    [TestMethod]
    public async Task ShowDirectorFromMovieAsync_Should_ReturnCorrectDirectorCount()
    {
        // ARRANGE  
        Setup();
        AppDBContext context = new AppDBContext(_dbContextOptions);
        MovieRepository movieRepository = new MovieRepository(context);
        MovieService movieService = new MovieService(movieRepository, null, context);

        Movie movie = new Movie
        {
            IMDBId = 1,
            Title = "Inception",
            MovieDirectors = new List<MovieDirector>
        {
            new MovieDirector
            {
                Director = new Director { Name = "Christopher Nolan" }
            }
        }
        };

        context.Movies.Add(movie);
        await context.SaveChangesAsync();

        // ACT
        IEnumerable<Director> directors = await movieService.ShowDirectorFromMovieAsync("Inception");

        // ASSERT
        Assert.AreEqual(1, directors.Count());
    }

    [TestMethod]
    public async Task ShowDirectorFromMovieAsync_Should_ThrowException_WhenMovieNotFound()
    {
        // ARRANGE  
        Setup();
        AppDBContext context = new AppDBContext(_dbContextOptions);
        MovieRepository movieRepository = new MovieRepository(context);
        MovieService movieService = new MovieService(movieRepository, null, context);

        // ACT & ASSERT
        await Assert.ThrowsExceptionAsync<InvalidOperationException>(async () =>
        {
            await movieService.ShowDirectorFromMovieAsync("Niet Bestaande Film");
        });
    }

    [TestMethod]
    public async Task ShowYearFromMovieAsync_Should_NotReturnNull()
    {
        // ARRANGE
        Setup();
        AppDBContext context = new AppDBContext(_dbContextOptions);
        MovieRepository movieRepository = new MovieRepository(context);
        MovieService movieService = new MovieService(movieRepository, null, context);

        Movie movie = new Movie { IMDBId = 1, Title = "Inception", Year = "2010" };

        context.Movies.Add(movie);
        await context.SaveChangesAsync();

        // ACT
        string year = await movieService.ShowYearFromMovieAsync("Inception");

        // ASSERT
        Assert.IsNotNull(year);
    }

    [TestMethod]
    public async Task ShowYearFromMovieAsync_Should_ReturnCorrectYear()
    {
        // ARRANGE  
        Setup();
        AppDBContext context = new AppDBContext(_dbContextOptions);
        MovieRepository movieRepository = new MovieRepository(context);
        MovieService movieService = new MovieService(movieRepository, null, context);

        Movie movie = new Movie { IMDBId = 1, Title = "Inception", Year = "2010" };

        context.Movies.Add(movie);
        await context.SaveChangesAsync();

        // ACT
        string year = await movieService.ShowYearFromMovieAsync("Inception");

        // ASSERT
        Assert.AreEqual("2010", year);
    }

    [TestMethod]
    public async Task ShowYearFromMovieAsync_Should_ThrowException_WhenMovieNotFound()
    {
        // ARRANGE  
        Setup();
        AppDBContext context = new AppDBContext(_dbContextOptions);
        MovieRepository movieRepository = new MovieRepository(context);
        MovieService movieService = new MovieService(movieRepository, null, context);

        // ACT
        Func<Task> action = async () => await movieService.ShowYearFromMovieAsync("Niet Bestaande Film");

        // Assert
        await Assert.ThrowsAsync<ArgumentException>(action);
    }

    #endregion

    #region testen op "Filter by model"

    [TestMethod]
    public async Task ShowMoviesFromTheSameActorAsync_Should_ReturnMovies_For_Actor()
    {
        // ARRANGE  
        Setup();
        AppDBContext context = new AppDBContext(_dbContextOptions);
        MovieRepository movieRepository = new MovieRepository(context);
        MovieService movieService = new MovieService(movieRepository, null, context);

        Actor actor = new Actor { Id = 1, Name = "Leonardo DiCaprio" };
        Movie movie1 = new Movie { IMDBId = 1, Title = "Inception", MovieActors = new List<MovieActor> 
        { 
            new MovieActor { Actor = actor } 
        }
        };
        Movie movie2 = new Movie { IMDBId = 2, Title = "Titanic", MovieActors = new List<MovieActor>
        {
            new MovieActor { Actor = actor }
        }
        };

        context.Movies.Add(movie1);
        context.Movies.Add(movie2);
        await context.SaveChangesAsync();

        // ACT
        IEnumerable<Movie> movies = await movieService.ShowMoviesFromTheSameActorAsync(actor);

        // ASSERT
        Assert.IsTrue(movies.Count() > 0);
    }

    [TestMethod]
    public async Task ShowMoviesFromTheSameActorAsync_Should_ReturnCorrectMovieCount()
    {
        // ARRANGE  
        Setup();
        AppDBContext context = new AppDBContext(_dbContextOptions);
        MovieRepository movieRepository = new MovieRepository(context);
        MovieService movieService = new MovieService(movieRepository, null, context);

        Actor actor = new Actor { Id = 1, Name = "Leonardo DiCaprio" };
        Movie movie1 = new Movie { IMDBId = 1, Title = "Inception", MovieActors = new List<MovieActor> 
        { 
            new MovieActor { Actor = actor } 
        }
        };
        Movie movie2 = new Movie { IMDBId = 2, Title = "Titanic", MovieActors = new List<MovieActor> 
        { 
            new MovieActor { Actor = actor } 
        }
        };

        context.Movies.Add(movie1);
        context.Movies.Add(movie2);
        await context.SaveChangesAsync();

        // ACT
        IEnumerable<Movie> movies = await movieService.ShowMoviesFromTheSameActorAsync(actor);

        // ASSERT
        Assert.AreEqual(2, movies.Count());
    }

    [TestMethod]
    public async Task ShowMoviesFromTheSameActorAsync_Should_ThrowException_When_ActorNotFound()
    {
        // ARRANGE  
        Setup();
        AppDBContext context = new AppDBContext(_dbContextOptions);
        MovieRepository movieRepository = new MovieRepository(context);
        MovieService movieService = new MovieService(movieRepository, null, context);

        Actor actor = new Actor { Id = 999, Name = "Non-Existent Actor" };

        // ACT & ASSERT
        await Assert.ThrowsExceptionAsync<InvalidOperationException>(async () =>
        {
            await movieService.ShowMoviesFromTheSameActorAsync(actor);
        });
    }

    [TestMethod]
    public async Task ShowMoviesFromTheSameDirectorAsync_Should_ReturnMovies_For_Director()
    {
        // ARRANGE  
        Setup();
        AppDBContext context = new AppDBContext(_dbContextOptions);
        MovieRepository movieRepository = new MovieRepository(context);
        MovieService movieService = new MovieService(movieRepository, null, context);

        Director director = new Director { Id = 1, Name = "Christopher Nolan" };
        Movie movie1 = new Movie { IMDBId = 1, Title = "Inception", MovieDirectors = new List<MovieDirector>
        {
            new MovieDirector { Director = director }
        }
        };
        Movie movie2 = new Movie
        {
            IMDBId = 2,
            Title = "Interstellar",
            MovieDirectors = new List<MovieDirector>
            { 
                new MovieDirector { Director = director }
            }
        };

        context.Movies.Add(movie1);
        context.Movies.Add(movie2);
        await context.SaveChangesAsync();

        // ACT
        IEnumerable<Movie> movies = await movieService.ShowMoviesFromTheSameDirectorAsync(director);

        // ASSERT
        Assert.IsTrue(movies.Count() > 0);
    }

    [TestMethod]
    public async Task ShowMoviesFromTheSameDirectorAsync_Should_ReturnCorrectMovieCount()
    {
        // ARRANGE  
        Setup();
        AppDBContext context = new AppDBContext(_dbContextOptions);
        MovieRepository movieRepository = new MovieRepository(context);
        MovieService movieService = new MovieService(movieRepository, null, context);

        Director director = new Director { Id = 1, Name = "Christopher Nolan" };
        Movie movie1 = new Movie { IMDBId = 1, Title = "Inception", MovieDirectors = new List<MovieDirector>
        {
            new MovieDirector { Director = director }
        }
        };

        Movie movie2 = new Movie { IMDBId = 2, Title = "Interstellar", MovieDirectors = new List<MovieDirector>
        {
            new MovieDirector { Director = director }
        }
        };

        context.Movies.Add(movie1);
        context.Movies.Add(movie2);
        await context.SaveChangesAsync();

        // ACT
        IEnumerable<Movie> movies = await movieService.ShowMoviesFromTheSameDirectorAsync(director);

        // ASSERT
        Assert.AreEqual(2, movies.Count());
    }

    [TestMethod]
    public async Task ShowMoviesFromTheSameDirectorAsync_Should_ThrowException_When_DirectorNotFound()
    {
        // ARRANGE  
        Setup();
        AppDBContext context = new AppDBContext(_dbContextOptions);
        MovieRepository movieRepository = new MovieRepository(context);
        MovieService movieService = new MovieService(movieRepository, null, context);

        Director director = new Director { Id = 999, Name = "Non-Existent Director" };

        // ACT & ASSERT
        await Assert.ThrowsExceptionAsync<InvalidOperationException>(async () =>
        {
            await movieService.ShowMoviesFromTheSameDirectorAsync(director);
        });
    }

    [TestMethod]
    public async Task ShowAllMoviesFromTheSameYearAsync_Should_ReturnMovies_For_Year()
    {
        // ARRANGE  
        Setup();
        AppDBContext context = new AppDBContext(_dbContextOptions);
        MovieRepository movieRepository = new MovieRepository(context);
        MovieService movieService = new MovieService(movieRepository, null, context);

        Movie movie1 = new Movie { IMDBId = 1, Title = "Inception", Year = "2010" };
        Movie movie2 = new Movie { IMDBId = 2, Title = "Interstellar", Year = "2010" };

        context.Movies.Add(movie1);
        context.Movies.Add(movie2);
        await context.SaveChangesAsync();

        // ACT
        IEnumerable<Movie> movies = await movieService.ShowAllMoviesFromTheSameYearAsync("2010");

        // ASSERT
        Assert.IsTrue(movies.Count() > 0);
    }

    [TestMethod]
    public async Task ShowAllMoviesFromTheSameYearAsync_Should_ReturnCorrectMovieCount()
    {
        // ARRANGE  
        Setup();
        AppDBContext context = new AppDBContext(_dbContextOptions);
        MovieRepository movieRepository = new MovieRepository(context);
        MovieService movieService = new MovieService(movieRepository, null, context);

        Movie movie1 = new Movie { IMDBId = 1, Title = "Inception", Year = "2010" };
        Movie movie2 = new Movie { IMDBId = 2, Title = "Interstellar", Year = "2010" };

        context.Movies.Add(movie1);
        context.Movies.Add(movie2);
        await context.SaveChangesAsync();

        // ACT
        IEnumerable<Movie> movies = await movieService.ShowAllMoviesFromTheSameYearAsync("2010");

        // ASSERT
        Assert.AreEqual(2, movies.Count());
    }

    [TestMethod]
    public async Task ShowAllMoviesFromTheSameYearAsync_Should_ReturnEmpty_When_NoMoviesForYear()
    {
        // ARRANGE  
        Setup();
        AppDBContext context = new AppDBContext(_dbContextOptions);
        MovieRepository movieRepository = new MovieRepository(context);
        MovieService movieService = new MovieService(movieRepository, null, context);

        // ACT
        IEnumerable<Movie> movies = await movieService.ShowAllMoviesFromTheSameYearAsync("1999");

        // ASSERT
        Assert.AreEqual(0, movies.Count());
    }

    [TestMethod]
    public async Task ShowAllMoviesFromTheSameYearAsync_Should_ThrowException_When_InvalidYear()
    {
        // ARRANGE  
        Setup();
        AppDBContext context = new AppDBContext(_dbContextOptions);
        MovieRepository movieRepository = new MovieRepository(context);
        MovieService movieService = new MovieService(movieRepository, null, context);

        // ACT
        Func<Task> action = async () => await movieService.ShowAllMoviesFromTheSameYearAsync("InvalidYear");

        // ASSERT
        await Assert.ThrowsAsync<ArgumentException>(action);
    }

    [TestMethod]
    public async Task ShowAllMoviesFromTheSameYearAsync_Should_ThrowException_When_Year_Does_Not_Contain_4_Digits()
    {
        // ARRANGE  
        Setup();
        AppDBContext context = new AppDBContext(_dbContextOptions);
        MovieRepository movieRepository = new MovieRepository(context);
        MovieService movieService = new MovieService(movieRepository, null, context);

        // ACT
        Func<Task> action = async () => await movieService.ShowAllMoviesFromTheSameYearAsync("201");

        // ASSERT
        await Assert.ThrowsAsync<ArgumentException>(action);


    }

    #endregion
}
