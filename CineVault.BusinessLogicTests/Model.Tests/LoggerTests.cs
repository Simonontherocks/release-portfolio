using CineVault.BusinessLogic.Models;

namespace CineVault.BusinessLogicTests.BusinessLogic.Model.Tests;

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
        logger.Id = 1;
        logger.TimeOfDay = now;
        logger.UserId = 42;
        logger.Description = "System start";
        logger.Type = LogType.user;

        // Assert
        Assert.AreEqual(1, logger.Id);
        Assert.AreEqual(now, logger.TimeOfDay);
        Assert.AreEqual(42, logger.UserId);
        Assert.AreEqual("System start", logger.Description);
        Assert.AreEqual(LogType.user, logger.Type);
    }

}
