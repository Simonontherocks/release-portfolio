using CineVault.DataAccessLayer.Context;
using CineVault.DataAccessLayer.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CineVault.BusinessLogicTests;

[TestClass]
public class ApiServiceTests
{
    #region Field

    private DbContextOptions<AppDBContext> _dbContextOptions;

    #endregion

    #region TestInitialize

    public void Setup()
    {
        // Nieuwe databasecontext per test
        _dbContextOptions = new DbContextOptionsBuilder<AppDBContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Unieke database per test
            .Options;
    }

    #endregion

    [TestMethod]
    public void TestMethod1()
    {
    }
}
