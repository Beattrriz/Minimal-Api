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

        public Admin? Login(LoginDTO loginDTO)
        {
            var adm = _context.Admins
                .Where(a => a.Email == loginDTO.Email && a.Password == loginDTO.Password)
                .FirstOrDefault();
            
            return adm; 
        }
    }
}