using CineVault.BusinessLogic;
using CineVault.BusinessLogic.Models;

namespace CineVault.BusinessLogicTests.BusinessLogic.Model.Tests
{
    [TestClass]
    public sealed class ActorTests
    {
        [TestMethod]
        public void Actor_CanBeInitialized()
        { // Arrange
            Actor actor = new Actor();

            // Act
            actor.Id = 1;
            actor.Name = "Leonardo DiCaprio";
            actor.IMDBEntry = new IMDBEntry { Name = "Leonardo DiCaprio", Url = "https://www.imdb.com/name/nm0000138/", Type = IMDBType.Actor };

            // Assert
            Assert.AreEqual(1, actor.Id);
            Assert.AreEqual("Leonardo DiCaprio", actor.Name);
            Assert.IsNotNull(actor.IMDBEntry);
            Assert.AreEqual("Leonardo DiCaprio", actor.IMDBEntry.Name);
        }

    }

}
