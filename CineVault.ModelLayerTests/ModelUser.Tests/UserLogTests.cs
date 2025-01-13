using CineVault.ModelLayer.ModelUser;

namespace CineVault.ModelLayerTests.ModelLayer.Tests;

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
        log.TimeOfDay = now;
        log.AmountOfUsers = 100;

        // Assert
        Assert.AreEqual(now, log.TimeOfDay);
        Assert.AreEqual(100, log.AmountOfUsers);
    }

}
