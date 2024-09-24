using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MinimalAPI;
using MinimalAPI.Domain.DTOs;
using MinimalAPI.Domain.Entities;
using MinimalAPI.Domain.Enuns;
using MinimalAPI.Domain.Interface;
using MinimalAPI.Domain.ModelViews;
using MinimalAPI.Domain.Service;
using MinimalAPI.Infra;

namespace MinimalAPI.Api;
public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
        key = Configuration.GetSection("Jwt").ToString() ?? "";
    }

    private string key = "";

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateLifetime = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                ValidateIssuer = false,
                ValidateAudience = false
            };
        });

        services.AddAuthorization();

        services.AddScoped<IAdminService, AdminService>();
        services.AddScoped<IVehicleService, VehicleService>();

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Insira o TokenJWT aqui"
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] {}
                }
            });
        });

        services.AddDbContext<DbContexto>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("ConexaoPadrao")));

        services.AddControllers();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            #region Home
            endpoints.MapGet("/", () => Results.Json(new Home())).AllowAnonymous().WithTags("Home");
            #endregion

            #region Admin

            string GenerateTokenJwt(Admin admin)
            {
                if (string.IsNullOrEmpty(key)) return string.Empty;

                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                var claims = new List<Claim>(){
                new Claim("Email", admin.Email),
                new Claim("Profile", admin.Profile),
                new Claim(ClaimTypes.Role, admin.Profile)
                };

                var token = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.Now.AddDays(1),
                    signingCredentials: credentials
                );

                return new JwtSecurityTokenHandler().WriteToken(token);
            }

            endpoints.MapPost("/login", ([FromBody] LoginDTO loginDTO, IAdminService adminService) =>
            {
                var adm = adminService.Login(loginDTO);

                if (adm != null)
                {
                    string token = GenerateTokenJwt(adm);
                    return Results.Ok(new AdminLogado
                    {
                        Email = adm.Email,
                        Profile = adm.Profile,
                        Token = token
                    });
                }
                else
                    return Results.Unauthorized();

            }).AllowAnonymous().WithTags("Administradores");


            endpoints.MapGet("/Administradores", ([FromQuery] int? pag, IAdminService adminService) =>
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
            })
            .RequireAuthorization()
            .RequireAuthorization(new AuthorizeAttribute { Roles = "Adm" })
            .WithTags("Administradores");


            endpoints.MapGet("/administradores/{id}", ([FromRoute] int id, IAdminService adminService) =>
            {
                var admin = adminService.SearchById(id);

                if (admin == null) return Results.NotFound();

                return Results.Ok(new AdminModelView
                {
                    Id = admin.Id,
                    Email = admin.Email,
                    Profile = admin.Profile
                });
            })
            .RequireAuthorization()
            .RequireAuthorization(new AuthorizeAttribute { Roles = "Adm" })
            .WithTags("Administradores");


            endpoints.MapPost("/Administradores", ([FromBody] AdminDTO adminDTO, IAdminService adminService) =>
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
            })
            .RequireAuthorization()
            .RequireAuthorization(new AuthorizeAttribute { Roles = "Adm" })
            .WithTags("Administradores");

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

            endpoints.MapPost("/veiculos", ([FromBody] VehicleDTO vehicleDTO, IVehicleService VehicleService) =>
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
            })
            .RequireAuthorization()
            .RequireAuthorization(new AuthorizeAttribute { Roles = "Adm, Editor" })
            .WithTags("Veiculos");


            endpoints.MapGet("/veiculos", ([FromQuery] int? pag, IVehicleService VehicleService) =>
            {
                var vehicles = VehicleService.All(pag);

                return Results.Ok(vehicles);
            }).RequireAuthorization().WithTags("Veiculos");


            endpoints.MapGet("/veiculos/{id}", ([FromRoute] int id, IVehicleService VehicleService) =>
            {
                var vehicle = VehicleService.SearchById(id);

                if (vehicle == null) return Results.NotFound();

                return Results.Ok(vehicle);
            })
            .RequireAuthorization()
            .RequireAuthorization(new AuthorizeAttribute { Roles = "Adm, Editor" })
            .WithTags("Veiculos");


            endpoints.MapPut("/veiculos/{id}", ([FromRoute] int id, VehicleDTO vehicleDTO, IVehicleService VehicleService) =>
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
            })
            .RequireAuthorization()
            .RequireAuthorization(new AuthorizeAttribute { Roles = "Adm" })
            .WithTags("Veiculos");


            endpoints.MapDelete("/veiculos/{id}", ([FromRoute] int id, IVehicleService VehicleService) =>
            {
                var vehicle = VehicleService.SearchById(id);

                if (vehicle == null) return Results.NotFound();

                VehicleService.Delete(vehicle);

                return Results.NoContent();
            })
            .RequireAuthorization()
            .RequireAuthorization(new AuthorizeAttribute { Roles = "Adm" })
            .WithTags("Veiculos");

            #endregion
        });

    }
}