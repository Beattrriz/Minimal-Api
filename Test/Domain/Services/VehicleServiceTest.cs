using Microsoft.EntityFrameworkCore;
using MinimalAPI.Domain.Entities;
using MinimalAPI.Domain.Service;

namespace Test.Domain.Entities;

[TestClass]
public class VehicleServiceTest
{
    [TestMethod]
    public void TestAddVehicle()
    {
        // Arrange
        using var context = TestDbContextFactory.CreateContextWithSqlServer();
        context.Database.ExecuteSqlRaw("TRUNCATE TABLE Vehicles");

        var vehicle = new Vehicle
        {
            Name = "Carro Teste",
            Mark = "Marca Teste",
            Year = 2024
        };

        var vehicleService = new VehicleService(context);

        // Act
        vehicleService.Add(vehicle);

        // Assert
        Assert.AreEqual(1, context.Vehicles.Count());
    }

    [TestMethod]
    public void TestAllVehicles()
    {
        // Arrange
        using var context = TestDbContextFactory.CreateContextWithSqlServer();
        context.Database.ExecuteSqlRaw("TRUNCATE TABLE Vehicles");

        // Adiciona veículos para testar a paginação
        for (int i = 1; i <= 15; i++)
        {
            var vehicle = new Vehicle
            {
                Name = $"Carro {i}",
                Mark = "Marca Teste",
                Year = 2024
            };
            context.Vehicles.Add(vehicle);
        }
        context.SaveChanges();

        var vehicleService = new VehicleService(context);

        // Act
        var resultFirstPage = vehicleService.All(1);
        var resultSecondPage = vehicleService.All(2);

        // Assert
        Assert.AreEqual(10, resultFirstPage.Count);
        Assert.AreEqual(5, resultSecondPage.Count);
    }

    [TestMethod]
    public void TestDeleteVehicle()
    {
        // Arrange
        using var context = TestDbContextFactory.CreateContextWithSqlServer();
        context.Database.ExecuteSqlRaw("TRUNCATE TABLE Vehicles");

        var vehicle = new Vehicle
        {
            Name = "Carro Teste",
            Mark = "Marca Teste",
            Year = 2024
        };
        context.Vehicles.Add(vehicle);
        context.SaveChanges();

        var vehicleService = new VehicleService(context);

        // Act
        vehicleService.Delete(vehicle);

        // Assert
        Assert.AreEqual(0, context.Vehicles.Count());
    }

    [TestMethod]
    public void TestSearchByIdVehicle()
    {
        // Arrange
        using var context = TestDbContextFactory.CreateContextWithSqlServer();
        context.Database.ExecuteSqlRaw("TRUNCATE TABLE Vehicles");

        var vehicle = new Vehicle
        {
            Name = "Carro Teste",
            Mark = "Marca Teste",
            Year = 2024
        };
        context.Vehicles.Add(vehicle);
        context.SaveChanges();

        var vehicleService = new VehicleService(context);

        // Act
        var result = vehicleService.SearchById(vehicle.Id);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(vehicle.Name, result?.Name);
    }

    [TestMethod]
    public void TestUpdateVehicle()
    {
        // Arrange
        using var context = TestDbContextFactory.CreateContextWithSqlServer();
        context.Database.ExecuteSqlRaw("TRUNCATE TABLE Vehicles");

        var vehicle = new Vehicle
        {
            Name = "Carro Teste",
            Mark = "Marca Teste",
            Year = 2024
        };
        context.Vehicles.Add(vehicle);
        context.SaveChanges();

        var vehicleService = new VehicleService(context);

        // Act
        vehicle.Name = "Carro Atualizado";
        vehicleService.Update(vehicle);

        // Assert
        var updatedVehicle = context.Vehicles.FirstOrDefault(v => v.Id == vehicle.Id);
        Assert.AreEqual("Carro Atualizado", updatedVehicle?.Name);
    }
}