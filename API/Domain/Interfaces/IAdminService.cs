using MinimalAPI.Domain.DTOs;
using MinimalAPI.Domain.Entities;

namespace MinimalAPI.Domain.Interface
{
    public interface IAdminService
    {
        Admin? Login(LoginDTO loginDTO);

        Admin Add(Admin admin);

        Admin? SearchById(int id);

        List<Admin> All(int? pag);
    }
}