using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MinimalAPI.Domain.DTOs;
using MinimalAPI.Domain.Entities;
using MinimalAPI.Domain.Service;
using MinimalAPI.Infra;

[TestClass]
public class AdminServiceTest
{

    [TestMethod]
    public void TestAddAdmin()
    {
        // Arrange
        using var context = TestDbContextFactory.CreateContextWithSqlServer();
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
        using var context = TestDbContextFactory.CreateContextWithSqlServer();
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

    [TestMethod]
    public void TestAllAdmins()
    {
        // Arrange
        using var context = TestDbContextFactory.CreateContextWithSqlServer();
        context.Database.ExecuteSqlRaw("TRUNCATE TABLE Admins");

        for (int i = 1; i <= 15; i++)
        {
            var adm = new Admin
            {
                Email = $"admin{i}@teste.com",
                Password = "password",
                Profile = "Adm"
            };
            context.Admins.Add(adm);
        }
        context.SaveChanges();

        var adminService = new AdminService(context);

        // Act
        var resultFirstPage = adminService.All(1);
        var resultSecondPage = adminService.All(2);

        // Assert
        Assert.AreEqual(10, resultFirstPage.Count);
        Assert.AreEqual(5, resultSecondPage.Count);

    }

    [TestMethod]
    public void TestAdminLogin_Successful()
    {
        // Arrange
        using var context = TestDbContextFactory.CreateContextWithSqlServer();
        context.Database.ExecuteSqlRaw("TRUNCATE TABLE Admins");

        var adm = new Admin
        {
            Email = "admin@teste.com",
            Password = "password123",
            Profile = "Adm"
        };
        context.Admins.Add(adm);
        context.SaveChanges();

        var loginDTO = new LoginDTO
        {
            Email = "admin@teste.com",
            Password = "password123"
        };

        var adminService = new AdminService(context);

        // Act
        var result = adminService.Login(loginDTO);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(adm.Email, result.Email);
    }

    [TestMethod]
    public void TestAdminLogin_Failure()
    {
        // Arrange
        using var context = TestDbContextFactory.CreateContextWithSqlServer();
        context.Database.ExecuteSqlRaw("TRUNCATE TABLE Admins");

        var adm = new Admin
        {
            Email = "admin@teste.com",
            Password = "password123",
            Profile = "Adm"
        };
        context.Admins.Add(adm);
        context.SaveChanges();

        var loginDTO = new LoginDTO
        {
            Email = "admin@teste.com",
            Password = "wrongpassword"
        };

        var adminService = new AdminService(context);

        // Act
        var result = adminService.Login(loginDTO);

        // Assert
        Assert.IsNull(result); 
    }
}


