using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MinimalAPI.Domain.DTOs;
using MinimalAPI.Domain.Interface;
using MinimalAPI.Domain.ModelViews;
using MinimalAPI.Domain.Service;
using MinimalAPI.Infra;

#region Builder

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IAdminService, AdminService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DbContexto>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConexaoPadrao")));

builder.Services.AddControllers();  

var app = builder.Build();
#endregion

#region Home
app.MapGet("/", () => Results.Json(new Home()));
#endregion

#region Admin
app.MapPost("/login", ([FromBody] LoginDTO loginDTO, IAdminService iAdminService) => {
    if(iAdminService.Login(loginDTO) != null)
    return Results.Ok("Login com sucesso");
    else
    return Results.Unauthorized();

    });
#endregion

#region Veiculo
app.MapPost("/veiculos", ([FromBody] LoginDTO loginDTO, IAdminService iAdminService) => {
    if(iAdminService.Login(loginDTO) != null)
    return Results.Ok("Login com sucesso");
    else
    return Results.Unauthorized();

    });
#endregion

#region App

app.UseSwagger();
app.UseSwaggerUI();

app.Run();
#endregion
