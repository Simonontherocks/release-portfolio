using CineVault.ModelLayer.ModelLayerService;

namespace CineVault.ModelLayerTests.ModelLayer;

[TestClass]
public class IdGeneratorTests
{
    [TestMethod]
    public void IdGenerator_GenerateId_ShouldIncrementId()
    {
        // Arrange
        int id1 = IdGeneratorService.GenerateId();

        // Act
        int id2 = IdGeneratorService.GenerateId();

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
            ids.Add(IdGeneratorService.GenerateId());
        }

        // Assert
        Assert.AreEqual(100, ids.Count);
    }

    [TestMethod]
    public void IdGenerator_GenerateId_ShouldBeSequential()
    {
        // Arrange
        int id1 = IdGeneratorService.GenerateId();
        int id2 = IdGeneratorService.GenerateId();

        // Act
        int nextId = IdGeneratorService.GenerateId();

        // Assert
        Assert.AreEqual(id2 + 1, nextId);
    }
    

    [TestMethod]
    public void IdGenerator_Reset_ShouldStartFromOne()
    {
        // Arrange
        IdGeneratorService.Reset();

        // Act
        int firstIdAfterReset = IdGeneratorService.GenerateId();

        // Assert
        Assert.AreEqual(1, firstIdAfterReset);
    }

}
