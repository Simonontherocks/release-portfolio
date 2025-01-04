using CineVault.BusinessLogic.ModelUser;

namespace CineVault.BusinessLogicTests.ModelUser.Tests;

[TestClass]
public class LoggerTests
{
    [TestMethod]
    public void Logger_CanBeInitialized()
    {
        // Arrange
        Logger logger = new Logger();
        DateTime now = DateTime.Now;

        // Act
        logger.TimeOfDay = now;
        logger.UserId = 42;
        logger.Description = "System start";
        logger.Type = LogType.user;

        // Assert
        Assert.AreEqual(now, logger.TimeOfDay);
        Assert.AreEqual(42, logger.UserId);
        Assert.AreEqual("System start", logger.Description);
        Assert.AreEqual(LogType.user, logger.Type);
    }

}
