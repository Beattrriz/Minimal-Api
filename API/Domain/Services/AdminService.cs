using MinimalAPI.Domain.DTOs;
using MinimalAPI.Domain.Entities;
using MinimalAPI.Domain.Interface;
using MinimalAPI.Infra;

namespace MinimalAPI.Domain.Service
{
    public class AdminService : IAdminService
    {
        private readonly DbContexto _context;

        public AdminService(DbContexto context)
        {
            _context = context;
        }

        public Admin Add(Admin admin)
        {
            _context.Admins.Add(admin);
            _context.SaveChanges();

            return admin;
        }

        public List<Admin> All(int? pag)
        {
            var query = _context.Admins.AsQueryable();

            int itemsPerPag = 10;

            if(pag != null)
            {
                query = query.Skip(( (int) pag - 1) * itemsPerPag).Take(itemsPerPag);
            }

            return query.ToList();
        }

        public Admin? Login(LoginDTO loginDTO)
        {
            var adm = _context.Admins
                .Where(a => a.Email == loginDTO.Email && a.Password == loginDTO.Password)
                .FirstOrDefault();
            
            return adm; 
        }

        public Admin? SearchById(int id)
        {
            return _context.Admins.Where(ad => ad.Id == id).FirstOrDefault();
        }
    }
}