
using MinimalAPI.Domain.DTOs;
using MinimalAPI.Domain.Entities;
using MinimalAPI.Domain.Interface;

namespace Test.Mocks;

public class AdminServiceMock : IAdminService
{
    private static List<Admin> administrators = new List<Admin>()
    {
        new Admin{
            Id = 1,
            Email = "adm@test.com",
            Password = "12345678",
            Profile = "Adm"
        },
        new Admin
        {
            Id = 2,
            Email = "editor@test.com",
            Password = "12345678",
            Profile = "Editor"
        }
    };
    
    public Admin Add(Admin admin)
    {
        admin.Id = administrators.Count() + 1;
        administrators.Add(admin);

        return admin;
    }

    public List<Admin> All(int? pag)
    {
        return administrators;
    }

    public Admin? Login(LoginDTO loginDTO)
    {
        return administrators.Find(a => a.Email == loginDTO.Email && a.Password == loginDTO.Password);
    }

    public Admin? SearchById(int id)
    {
        return administrators.Find(a => a.Id == id);
    }
}