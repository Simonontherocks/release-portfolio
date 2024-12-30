using CineVault.BusinessLogic.Models;

namespace CineVault.BusinessLogicTests.BusinessLogic.Model.Tests;

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
        user.Id = 1;
        user.Name = "John Doe";
        user.Email = "johndoe@example.com";
        user.PasswordHash = "hashedpassword";
        user.DateOfCreation = now;

        // Assert
        Assert.AreEqual(1, user.Id);
        Assert.AreEqual("John Doe", user.Name);
        Assert.AreEqual("johndoe@example.com", user.Email);
        Assert.AreEqual("hashedpassword", user.PasswordHash);
        Assert.AreEqual(now, user.DateOfCreation);
    }

}
