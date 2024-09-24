using Microsoft.EntityFrameworkCore;
using MinimalAPI.Domain.Entities;
using MinimalAPI.Domain.Interface;
using MinimalAPI.Infra;

namespace MinimalAPI.Domain.Service
{
    public class VehicleService : IVehicleService
    {
        private readonly DbContexto _context;
        public VehicleService(DbContexto context)
        {
             _context = context;
        }
        public void Add(Vehicle vehicle)
        {
            _context.Vehicles.Add(vehicle);
            _context.SaveChanges();
        }

        public List<Vehicle> All(int? pag = 1, string? name = null, string? mark = null)
        {
            var query = _context.Vehicles.AsQueryable();

            if(!string.IsNullOrEmpty(name))
            {
                query = query.Where(v => EF.Functions.Like(v.Name.ToLower(), $"%{name.ToLower()}%"));
            }

            int itemsPerPag = 10;

            if(pag != null)
            {
                query = query.Skip(( (int) pag - 1) * itemsPerPag).Take(itemsPerPag);
            }

            return query.ToList();
        }

        public void Delete(Vehicle vehicle)
        {
            _context.Vehicles.Remove(vehicle);
            _context.SaveChanges();
        }

        public Vehicle? SearchById(int id)
        {
            return _context.Vehicles.Where(v => v.Id == id).FirstOrDefault();
        }

        public void Update(Vehicle vehicle)
        {
            _context.Vehicles.Update(vehicle);
            _context.SaveChanges();
        }
    }
}