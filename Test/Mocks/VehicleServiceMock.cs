using MinimalAPI.Domain.Entities;
using MinimalAPI.Domain.Interface;

namespace Test.Mocks;

public class VehicleServiceMock : IVehicleService
{
    private static List<Vehicle> vehicles = new List<Vehicle>
        {
            new Vehicle { Id = 1, Name = "Carro 1", Mark = "Marca A", Year = 2024 },
            new Vehicle { Id = 2, Name = "Carro 2", Mark = "Marca B", Year = 2022 },
            new Vehicle { Id = 3, Name = "Carro 3", Mark = "Marca A", Year = 2023 }
        };
    public void Add(Vehicle vehicle)
    {
         vehicle.Id = vehicles.Count + 1; 
         vehicles.Add(vehicle);
    }

    public List<Vehicle> All(int? pag = 1, string? name = null, string? mark = null)
    {
        return vehicles;
    }

    public void Delete(Vehicle vehicle)
    {
        vehicles.Remove(vehicle);
    }

    public Vehicle? SearchById(int id)
    {
        return vehicles.Find(v => v.Id == id);
    }

    public void Update(Vehicle vehicle)
    {
        var existingVehicle = SearchById(vehicle.Id);
            
            if (existingVehicle != null)
            {
                existingVehicle.Name = vehicle.Name;
                existingVehicle.Mark = vehicle.Mark;
            }
    }
}
