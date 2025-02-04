using CineVault.ModelLayer.ModelMovie;

namespace CineVault.ModelLayerTests.ModelLayer.Model.Tests
{
    [TestClass]
    public class ActorTests
    {
        [TestMethod]
        public void Actor_CanBeInitialized()
        { // Arrange
            Actor actor = new Actor();

            // Act
            actor.Name = "Leonardo DiCaprio";

            // Assert
            Assert.AreEqual("Leonardo DiCaprio", actor.Name);
        }

    }

}
