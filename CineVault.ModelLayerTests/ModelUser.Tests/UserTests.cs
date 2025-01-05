using CineVault.BusinessLogic.ModelUser;

namespace CineVault.ModelLayerTests.ModelLayer.Tests;

[TestClass]
public class UserTests
{
    [TestMethod]
    public void User_CanBeInitialized()
    {
        // Arrange
        User user = new User();
        DateTime now = DateTime.Now;

        // Act
        user.Name = "John Doe";
        user.Email = "johndoe@example.com";
        user.PasswordHash = "hashedpassword";
        user.DateOfCreation = now;

        // Assert
        Assert.AreEqual("John Doe", user.Name);
        Assert.AreEqual("johndoe@example.com", user.Email);
        Assert.AreEqual("hashedpassword", user.PasswordHash);
        Assert.AreEqual(now, user.DateOfCreation);
    }

}
