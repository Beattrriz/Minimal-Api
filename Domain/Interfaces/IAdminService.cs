using MinimalAPI.Domain.DTOs;
using MinimalAPI.Domain.Entities;

namespace MinimalAPI.Domain.Interface
{
    public interface IAdminService
    {
        Admin? Login(LoginDTO loginDTO);
    }
}