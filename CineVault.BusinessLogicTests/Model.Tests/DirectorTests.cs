using CineVault.BusinessLogic.Models;

namespace CineVault.BusinessLogicTests.BusinessLogic.Model.Tests;

[TestClass]
public class DirectorTests
{
    [TestMethod]
    public void Director_CanBeInitialized()
    {
        // Arrange
        Director director = new Director();

        // Act
        director.Id = 1;
        director.Name = "Christopher Nolan";
        director.IMDBEntry = new IMDBEntry { Name = "Christopher Nolan", Url = "https://www.imdb.com/name/nm0634240/", Type = IMDBType.Director };

        // Assert
        Assert.AreEqual(1, director.Id);
        Assert.AreEqual("Christopher Nolan", director.Name);
        Assert.IsNotNull(director.IMDBEntry);
        Assert.AreEqual("Christopher Nolan", director.IMDBEntry.Name);
    }

}
