using CineVault.BusinessLogic.Models;

namespace CineVault.BusinessLogicTests.BusinessLogic.Model.Tests;

[TestClass]
public class MovieTests
{
    [TestMethod]
    public void Movie_CanBeInitialized()
    {
        // Arrange
        Movie movie = new Movie();
        Director director = new Director { Id = 1, Name = "Christopher Nolan" };
        List<Actor> actors = new List<Actor>
        {
        new Actor { Id = 1, Name = "Leonardo DiCaprio" },
        new Actor { Id = 2, Name = "Joseph Gordon-Levitt" }
        };

        IMDBEntry imdbEntry = new IMDBEntry { Name = "Inception", Url = "https://www.imdb.com/title/tt1375666/", Type = IMDBType.Movie };

        // Act
        movie.Id = 1;
        movie.Title = "Inception";
        movie.Barcode = "123456789";
        movie.Director = director;
        movie.Actors = actors;
        movie.IMDBEntry = imdbEntry;
        movie.Seen = true;
        movie.Score = 10;
        movie.Year = "2010";

        // Assert
        Assert.AreEqual(1, movie.Id);
        Assert.AreEqual("Inception", movie.Title);
        Assert.AreEqual("123456789", movie.Barcode);
        Assert.AreEqual(director, movie.Director);
        Assert.AreEqual(actors, movie.Actors);
        Assert.AreEqual(imdbEntry, movie.IMDBEntry);
        Assert.IsTrue(movie.Seen);
        Assert.AreEqual(10, movie.Score);
        Assert.AreEqual("2010", movie.Year);
    }

}
