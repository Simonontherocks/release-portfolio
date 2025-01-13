using CineVault.ModelLayer.ModelMovie;

namespace CineVault.ModelLayerTests.ModelLayer.Model.Tests;

[TestClass]
public class IMDBEntryTests
{
    [TestMethod]
    public void IMDBEntry_CanBeInitialized()
    {
        // Arrange
        IMDBEntry entry = new IMDBEntry();

        // Act
        entry.Name = "Inception";
        entry.Url = "https://www.imdb.com/title/tt1375666/";
        entry.Type = IMDBType.Movie;

        // Assert
        Assert.AreEqual("Inception", entry.Name);
        Assert.AreEqual("https://www.imdb.com/title/tt1375666/", entry.Url);
        Assert.AreEqual(IMDBType.Movie, entry.Type);
    }

}
