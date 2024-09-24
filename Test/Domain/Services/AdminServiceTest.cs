using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MinimalAPI.Domain.Entities;
using MinimalAPI.Domain.Service;
using MinimalAPI.Infra;

[TestClass]
public class AdminServiceTest
{
    private DbContexto CreateContextWithSqlServer()
    {

        var assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        var path = Path.GetFullPath(Path.Combine(assemblyPath ?? "", "..", "..", ".."));

        var configuration = new ConfigurationBuilder()
            .SetBasePath(path)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddEnvironmentVariables();

        var config = configuration.Build();

        var connectionString = config.GetConnectionString("ConexaoPadrao");

        var options = new DbContextOptionsBuilder<DbContexto>()
            .UseSqlServer(connectionString)
            .Options;

        return new DbContexto(options, config);
    }

    [TestMethod]
    public void TestAddAdmin()
    {
        // Arrange
        using var context = CreateContextWithSqlServer();
        context.Database.ExecuteSqlRaw("TRUNCATE TABLE Admins");

        var adm = new Admin
        {
            Email = "teste@teste.com",
            Password = "teste",
            Profile = "Adm"
        };

        var adminService = new AdminService(context);

        // Act
        adminService.Add(adm);

        // Assert
        Assert.AreEqual(1, adminService.All(1).Count());
    }

    public void TestSearchByIdAdmin()
    {
        // Arrange
        using var context = CreateContextWithSqlServer();
        context.Database.ExecuteSqlRaw("TRUNCATE TABLE Admins");

        var adm = new Admin
        {
            Email = "teste@teste.com",
            Password = "teste",
            Profile = "Adm"
        };

        var adminService = new AdminService(context);

        // Act
        adminService.Add(adm);
        adminService.SearchById(adm.Id);

        // Assert
        Assert.AreEqual(1, adm.Id);
    }
}


