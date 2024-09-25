using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using MinimalAPI.Api;
using MinimalAPI.Domain.Interface;
using Test.Mocks;

namespace Test.Helpers;

public class Setup
{
    public const string PORT = "5285";
    public static TestContext testContext = default!;
    public static WebApplicationFactory<Startup> http = default!;
    public static HttpClient client = default!;

    public static void ClassInit(TestContext testContext)
    {
        Setup.testContext = testContext;
        Setup.http = new WebApplicationFactory<Startup>();
        Setup.http = Setup.http.WithWebHostBuilder(builder =>
        {
            builder.UseSetting("http_port", Setup.PORT).UseEnvironment("Testing");

            builder.ConfigureServices(services =>
            {
                services.AddScoped<IAdminService, AdminServiceMock>();
                services.AddScoped<IVehicleService, VehicleServiceMock>();
            });
        });

        Setup.client = Setup.http.CreateClient();
    }

    public static void ClassCleanup()
    {
        Setup.http.Dispose();
    }
}