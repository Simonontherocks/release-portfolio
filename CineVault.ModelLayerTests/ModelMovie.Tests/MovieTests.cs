using CineVault.ModelLayer.ModelMovie;

namespace CineVault.ModelLayerTests.ModelLayer.Model.Tests;

[TestClass]
public class MovieTests
{
    [TestMethod]
    public void Movie_HasCorrectTitle()
    {
        // Arrange
        Movie movie = new Movie();

        // Act
        movie.Title = "Inception";

        // Assert
        Assert.AreEqual("Inception", movie.Title);
    }

    [TestMethod]
    public void Movie_HasBeenSeen()
    {
        // Arrange
        Movie movie = new Movie();

        // Act
        movie.Seen = true;

        // Assert
        Assert.IsTrue(movie.Seen);
    }

    [TestMethod]
    public void Movie_HasCorrectScore()
    {
        // Arrange
        Movie movie = new Movie();

        // Act
        movie.Score = 10;

        // Assert
        Assert.AreEqual(10, movie.Score);
    }

    [TestMethod]
    public void Movie_HasCorrectYear()
    {
        // Arrange
        Movie movie = new Movie();

        // Act
        movie.Year = "2010";

        // Assert
        Assert.AreEqual("2010", movie.Year);
    }

}
