using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MinimalAPI.Domain.DTOs;
using MinimalAPI.Domain.Entities;
using MinimalAPI.Domain.Enuns;
using MinimalAPI.Domain.Interface;
using MinimalAPI.Domain.ModelViews;
using MinimalAPI.Domain.Service;
using MinimalAPI.Infra;

#region Builder

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IAdminService, AdminService>();
builder.Services.AddScoped<IVehicleService, VehicleService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DbContexto>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConexaoPadrao")));

builder.Services.AddControllers();

var app = builder.Build();
#endregion

#region Home
app.MapGet("/", () => Results.Json(new Home())).WithTags("Home");
#endregion

#region Admin

app.MapPost("/login", ([FromBody] LoginDTO loginDTO, IAdminService adminService) =>
{
    if (adminService.Login(loginDTO) != null)
        return Results.Ok("Login com sucesso");
    else
        return Results.Unauthorized();

}).WithTags("Administradores");


app.MapGet("/Administradores", ([FromQuery] int? pag, IAdminService adminService) =>
{
    var adms = new List<AdminModelView>();
    var administrators = adminService.All(pag);

    foreach (var adm in administrators)
    {
        adms.Add(new AdminModelView
        {
            Id = adm.Id,
            Email = adm.Email,
            Profile = adm.Profile
        });
    }
    return Results.Ok(adms);
}).WithTags("Administradores");


app.MapGet("/administradores/{id}", ([FromRoute] int id, IAdminService adminService) =>
{
    var admin = adminService.SearchById(id);

    if (admin == null) return Results.NotFound();

    return Results.Ok(new AdminModelView
    {
        Id = admin.Id,
        Email = admin.Email,
        Profile = admin.Profile
    });
}).WithTags("Administradores");


app.MapPost("/Administradores", ([FromBody] AdminDTO adminDTO, IAdminService adminService) =>
{
    var validation = new ErrorValidation
    {
        Messages = new List<string>()
    };

    if (string.IsNullOrEmpty(adminDTO.Email))
        validation.Messages.Add("O Email não pode ser vazio");

    if (string.IsNullOrEmpty(adminDTO.Password))
        validation.Messages.Add("A Senha não pode ser vazia");

    if (adminDTO.Profile == null)
        validation.Messages.Add("O Perfil não pode ser vazio");

    if (validation.Messages.Count > 0)
        return Results.BadRequest(validation);

    var admin = new Admin
    {
        Email = adminDTO.Email,
        Password = adminDTO.Password,
        Profile = adminDTO.Profile.ToString() ?? Profile.Editor.ToString()
    };

    adminService.Add(admin);

    return Results.Created($"/administrador/{admin.Id}", new AdminModelView
    {
        Id = admin.Id,
        Email = admin.Email,
        Profile = admin.Profile
    });
}).WithTags("Administradores");
#endregion

#region Veiculo

ErrorValidation validaDTO(VehicleDTO vehicleDTO)
{
    var validation = new ErrorValidation
    {
        Messages = new List<string>()
    };

    if (string.IsNullOrEmpty(vehicleDTO.Name))
        validation.Messages.Add("O nome não pode ser vazio");

    if (string.IsNullOrEmpty(vehicleDTO.Mark))
        validation.Messages.Add("A marca não pode ficar em branco");

    if (vehicleDTO.Year < 1950)
        validation.Messages.Add("Veiculo muito antigo, aceito somente anos superiores a 1950");

    return validation;
}

app.MapPost("/veiculos", ([FromBody] VehicleDTO vehicleDTO, IVehicleService VehicleService) =>
{
    var validation = validaDTO(vehicleDTO);
    if (validation.Messages.Count > 0)
        return Results.BadRequest(validation);

    var vehicle = new Vehicle
    {
        Name = vehicleDTO.Name,
        Mark = vehicleDTO.Mark,
        Year = vehicleDTO.Year
    };
    VehicleService.Add(vehicle);

    return Results.Created($"/veiculo/{vehicle.Id}", vehicle);
}).WithTags("Veiculos");


app.MapGet("/veiculos", ([FromQuery] int? pag, IVehicleService VehicleService) =>
{
    var vehicles = VehicleService.All(pag);

    return Results.Ok(vehicles);
}).WithTags("Veiculos");


app.MapGet("/veiculos/{id}", ([FromRoute] int id, IVehicleService VehicleService) =>
{
    var vehicle = VehicleService.SearchById(id);

    if (vehicle == null) return Results.NotFound();

    return Results.Ok(vehicle);
}).WithTags("Veiculos");


app.MapPut("/veiculos/{id}", ([FromRoute] int id, VehicleDTO vehicleDTO, IVehicleService VehicleService) =>
{

    var vehicle = VehicleService.SearchById(id);

    if (vehicle == null) return Results.NotFound();

    var validation = validaDTO(vehicleDTO);
    if (validation.Messages.Count > 0)
        return Results.BadRequest(validation);

    vehicle.Name = vehicleDTO.Name;
    vehicle.Mark = vehicleDTO.Mark;
    vehicle.Year = vehicleDTO.Year;

    VehicleService.Update(vehicle);

    return Results.Ok(vehicle);
}).WithTags("Veiculos");


app.MapDelete("/veiculos/{id}", ([FromRoute] int id, IVehicleService VehicleService) =>
{
    var vehicle = VehicleService.SearchById(id);

    if (vehicle == null) return Results.NotFound();

    VehicleService.Delete(vehicle);

    return Results.NoContent();
}).WithTags("Veiculos");

#endregion

#region App

app.UseSwagger();
app.UseSwaggerUI();

app.Run();
#endregion
