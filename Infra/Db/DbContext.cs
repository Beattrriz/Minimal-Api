using Microsoft.EntityFrameworkCore;
using minimal_api.Migrations;
using MinimalAPI.Domain.Entities;

namespace MinimalAPI.Infra
{
    public class DbContexto : DbContext
{
    private readonly IConfiguration _configurationAppSettings;

    public DbContexto(IConfiguration configurationAppSettings)
    {
        _configurationAppSettings = configurationAppSettings;
    }

    public DbSet<Admin> Admins { get; set; } = default!;
    public DbSet<Vehicle> Vehicles {get; set;} = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Admin>().HasData(
                new Admin{
                    Id = 1,
                    Email = "admin@teste.com",
                    Password = "12345678",
                    Profile = "Adm"
                }
            );
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var connectionString = _configurationAppSettings.GetConnectionString("ConexaoPadrao");
            optionsBuilder.UseSqlServer(connectionString);
        }
    }
}
}