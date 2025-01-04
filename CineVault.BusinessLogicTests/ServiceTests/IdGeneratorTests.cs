using CineVault.BusinessLogic.Service;

namespace CineVault.BusinessLogicTests;

[TestClass]
public class IdGeneratorTests
{
    [TestMethod]
    public void IdGenerator_GenerateId_ShouldIncrementId()
    {
        // Arrange
        int id1 = IdGenerator.GenerateId();

        // Act
        int id2 = IdGenerator.GenerateId();

        // Assert
        Assert.AreEqual(id1 + 1, id2);
    }

    [TestMethod]
    public void IdGenerator_GenerateId_ShouldReturnUniqueIds()
    {
        // Arrange
        HashSet<int> ids = new HashSet<int>();

        // Act
        for (int i = 0; i < 100; i++)
        {
            ids.Add(IdGenerator.GenerateId());
        }

        // Assert
        Assert.AreEqual(100, ids.Count);
    }

    [TestMethod]
    public void IdGenerator_GenerateId_ShouldBeSequential()
    {
        // Arrange
        int id1 = IdGenerator.GenerateId();
        int id2 = IdGenerator.GenerateId();

        // Act
        int nextId = IdGenerator.GenerateId();

        // Assert
        Assert.AreEqual(id2 + 1, nextId);
    }

    [TestMethod]
    public void IdGenerator_GenerateId_ShouldStartFromOne()
    {
        // Arrange & Act
        int firstId = IdGenerator.GenerateId();

        // Assert
        Assert.AreEqual(1, firstId);
    }

    [TestMethod]
    public void IdGenerator_Reset_ShouldStartFromOne()
    {
        // Assuming there is a Reset method in IdGenerator class

        // Arrange
        IdGenerator.Reset();

        // Act
        var firstIdAfterReset = IdGenerator.GenerateId();

        // Assert
        Assert.AreEqual(1, firstIdAfterReset);
    }
}
