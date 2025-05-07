using CineVault.ModelLayer.ModelUser;

namespace CineVault.ModelLayerTests.ModelLayer.Tests;

[TestClass]
public class UserLogTests
{
    [TestMethod]
    public void UserLog_HasCorrectTimeOfDay()
    {
        // Arrange
        UserLog log = new UserLog();
        DateTime now = DateTime.Now;

        // Act
        log.TimeOfDay = now;

        // Assert
        Assert.AreEqual(now, log.TimeOfDay);
    }

    [TestMethod]
    public void UserLog_HasCorrectAmountOfUsers()
    {
        // Arrange
        UserLog log = new UserLog();

        // Act
        log.AmountOfUsers = 100;

        // Assert
        Assert.AreEqual(100, log.AmountOfUsers);
    }
}
