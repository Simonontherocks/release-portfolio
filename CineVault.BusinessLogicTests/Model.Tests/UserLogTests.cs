using CineVault.BusinessLogic.Models;

namespace CineVault.BusinessLogicTests.BusinessLogic.Model.Tests;

[TestClass]
public class UserLogTests
{
    [TestMethod]
    public void UserLog_CanBeInitialized()
    {
        // Arrange
        UserLog log = new UserLog();
        DateTime now = DateTime.Now;

        // Act
        log.Id = 1;
        log.TimeOfDay = now;
        log.AmountOfUsers = 100;

        // Assert
        Assert.AreEqual(1, log.Id);
        Assert.AreEqual(now, log.TimeOfDay);
        Assert.AreEqual(100, log.AmountOfUsers);
    }

}
