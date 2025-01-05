using CineVault.BusinessLogic;
using CineVault.BusinessLogic.ModelMovie;

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
            actor.IMDBEntry = new IMDBEntry { Name = "Leonardo DiCaprio", Url = "https://www.imdb.com/name/nm0000138/", Type = IMDBType.Actor };

            // Assert
            Assert.AreEqual("Leonardo DiCaprio", actor.Name);
            Assert.IsNotNull(actor.IMDBEntry);
            Assert.AreEqual("Leonardo DiCaprio", actor.IMDBEntry.Name);
        }

    }

}
