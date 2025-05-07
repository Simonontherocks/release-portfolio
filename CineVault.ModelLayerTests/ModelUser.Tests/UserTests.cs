using CineVault.ModelLayer.ModelUser;

namespace CineVault.ModelLayerTests.ModelLayer.Tests;

[TestClass]
public class UserTests
{
    [TestMethod]
    public void User_HasCorrectName()
    {
        // Arrange
        User user = new User();

        // Act
        user.Name = "John Doe";

        // Assert
        Assert.AreEqual("John Doe", user.Name);
    }

    [TestMethod]
    public void User_HasCorrectEmail()
    {
        // Arrange
        User user = new User();

        // Act
        user.Email = "johndoe@example.com";

        // Assert
        Assert.AreEqual("johndoe@example.com", user.Email);
    }

    [TestMethod]
    public void User_HasCorrectPasswordHash()
    {
        // Arrange
        User user = new User();

        // Act
        user.PasswordHash = "hashedpassword";

        // Assert
        Assert.AreEqual("hashedpassword", user.PasswordHash);
    }

    [TestMethod]
    public void User_HasCorrectDateOfCreation()
    {
        // Arrange
        User user = new User();
        DateTime now = DateTime.Now;

        // Act
        user.DateOfCreation = now;

        // Assert
        Assert.AreEqual(now, user.DateOfCreation);
    }
}
