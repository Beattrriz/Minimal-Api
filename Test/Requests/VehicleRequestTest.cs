using System.Net;
using System.Text;
using System.Text.Json;
using MinimalAPI.Domain.DTOs;
using MinimalAPI.Domain.Entities;
using Test.Helpers;

namespace Test.Request;

public class VehicleRequestTest
{
    [ClassInitialize]

    public static void ClassInit(TestContext testContext)
    {
        Setup.ClassInit(testContext);
    }

    [TestMethod]
        public async Task TestPostAddVehicle()
        {
            // Arrange
            var newVehicle = new VehicleDTO
            {
                Name = "Carro Teste",
                Mark = "Marca Teste",
                Year = 2020
            };

            var content = new StringContent(JsonSerializer.Serialize(newVehicle), Encoding.UTF8, "application/json");

            // Act
            var response = await Setup.client.PostAsync("/veiculos", content);

            // Assert
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);

            var result = await response.Content.ReadAsStringAsync();
            var addedVehicle = JsonSerializer.Deserialize<Vehicle>(result, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            Assert.IsNotNull(addedVehicle);
            Assert.AreEqual(newVehicle.Name, addedVehicle.Name);
            Assert.AreEqual(newVehicle.Mark, addedVehicle.Mark);
        }

        [TestMethod]
        public async Task TestGetAllVehicles()
        {
            // Act
            var response = await Setup.client.GetAsync("/veiculos");

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var result = await response.Content.ReadAsStringAsync();
            var vehicles = JsonSerializer.Deserialize<List<Vehicle>>(result, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            Assert.IsNotNull(vehicles);
            Assert.IsTrue(vehicles.Count > 0); 
        }

        [TestMethod]
        public async Task TestGetVehicleById()
        {
            // Act
            var response = await Setup.client.GetAsync("/veiculos/1");

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var result = await response.Content.ReadAsStringAsync();
            var vehicle = JsonSerializer.Deserialize<Vehicle>(result, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            Assert.IsNotNull(vehicle);
            Assert.AreEqual(1, vehicle.Id); 
        }

        [TestMethod]
        public async Task TestPutUpdateVehicle()
        {
            // Arrange
            var updatedVehicle = new VehicleDTO
            {
                Name = "Carro Atualizado",
                Mark = "Marca Atualizada",
                Year = 2021
            };

            var content = new StringContent(JsonSerializer.Serialize(updatedVehicle), Encoding.UTF8, "application/json");

            // Act
            var response = await Setup.client.PutAsync("/veiculos/1", content);

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var result = await response.Content.ReadAsStringAsync();
            var vehicle = JsonSerializer.Deserialize<Vehicle>(result, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            Assert.IsNotNull(vehicle);
            Assert.AreEqual(updatedVehicle.Name, vehicle.Name);
            Assert.AreEqual(updatedVehicle.Mark, vehicle.Mark);
        }

        [TestMethod]
        public async Task TestDeleteVehicle()
        {
            // Act
            var response = await Setup.client.DeleteAsync("/veiculos/1");

            // Assert
            Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);
        }
}