using CineVault.ModelLayer.ModelMovie;

namespace CineVault.ModelLayerTests.ModelLayer.Model.Tests;

[TestClass]
public class MovieTests
{
    [TestMethod]
    public void Movie_CanBeInitialized()
    {
        // Arrange
        Movie movie = new Movie();
        Director director = new Director { Name = "Christopher Nolan" };
        List<Actor> actors = new List<Actor>
        {
        new Actor { Name = "Leonardo DiCaprio" },
        new Actor { Name = "Joseph Gordon-Levitt" }
        };

        //IMDBEntry imdbEntry = new IMDBEntry { Name = "Inception", Url = "https://www.imdb.com/title/tt1375666/", Type = IMDBType.Movie };

        // Act
        movie.Title = "Inception";
        movie.Seen = true;
        movie.Score = 10;
        movie.Year = "2010";

        // Assert
        Assert.AreEqual("Inception", movie.Title);
        Assert.IsTrue(movie.Seen);
        Assert.AreEqual(10, movie.Score);
        Assert.AreEqual("2010", movie.Year);
    }

}
