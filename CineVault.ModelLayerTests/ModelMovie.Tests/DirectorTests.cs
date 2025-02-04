using CineVault.ModelLayer.ModelMovie;

namespace CineVault.ModelLayerTests.ModelLayer.Model.Tests;

[TestClass]
public class DirectorTests
{
    [TestMethod]
    public void Director_CanBeInitialized()
    {
        // Arrange
        Director director = new Director();

        // Act
        director.Name = "Christopher Nolan";

        // Assert
        Assert.AreEqual("Christopher Nolan", director.Name);
    }

}
