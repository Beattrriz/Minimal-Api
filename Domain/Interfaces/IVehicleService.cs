using MinimalAPI.Domain.Entities;

namespace MinimalAPI.Domain.Interface
{
    public interface IVehicleService
    {
        List<Vehicle> All (int pag = 1, string? name = null, string? mark = null);

        Vehicle? SearchById(int id);

        void Add (Vehicle vehicle);
        void Update (Vehicle vehicle);
        void Delete (Vehicle vehicle);
    }
}