using CineVault.ModelLayer.ModelUser;

namespace CineVault.ModelLayerTests.ModelLayer.Tests;

[TestClass]
public class LoggerTests
{
    [TestMethod]
    public void Logger_HasCorrectTimeOfDay()
    {
        // Arrange
        Logger logger = new Logger();
        DateTime now = DateTime.Now;

        // Act
        logger.TimeOfDay = now;

        // Assert
        Assert.AreEqual(now, logger.TimeOfDay);
    }

    [TestMethod]
    public void Logger_HasCorrectUserId()
    {
        // Arrange
        Logger logger = new Logger();

        // Act
        logger.UserId = 42;

        // Assert
        Assert.AreEqual(42, logger.UserId);
    }

    [TestMethod]
    public void Logger_HasCorrectDescription()
    {
        // Arrange
        Logger logger = new Logger();

        // Act
        logger.Description = "System start";

        // Assert
        Assert.AreEqual("System start", logger.Description);
    }

    [TestMethod]
    public void Logger_HasCorrectType()
    {
        // Arrange
        Logger logger = new Logger();

        // Act
        logger.Type = LogType.user;

        // Assert
        Assert.AreEqual(LogType.user, logger.Type);
    }

}
