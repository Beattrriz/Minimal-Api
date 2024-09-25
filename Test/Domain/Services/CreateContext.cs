using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MinimalAPI.Infra;

public static class TestDbContextFactory
{
    public static DbContexto CreateContextWithSqlServer()
    {
        var assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        var path = Path.GetFullPath(Path.Combine(assemblyPath ?? "", "..", "..", ".."));

        var configuration = new ConfigurationBuilder()
            .SetBasePath(path)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddEnvironmentVariables();

        var config = configuration.Build();
        var connectionString = config.GetConnectionString("ConexaoPadrao");

        var options = new DbContextOptionsBuilder<DbContexto>()
            .UseSqlServer(connectionString)
            .Options;

        return new DbContexto(options, config);
    }
}