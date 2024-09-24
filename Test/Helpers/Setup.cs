using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using MinimalAPI.Infra;
using MinimalAPI.Api;
using Microsoft.Extensions.DependencyInjection;
using MinimalAPI.Domain.Entities;

namespace API.Test.Helpers;

public class Setup
{
    public const string PORT = "5285";
    public static TestContext testContext = default!;
    public static WebApplicationFactory<Startup> http = default!;
    public HttpClient client = default!;

    public static void ClassInit(TestContext testContext)
    {
        Setup.testContext = testContext;
        Setup.http = new WebApplicationFactory<Startup>();
        Setup.http = Setup.http.WithWebHostBuilder(builder =>
        {
            builder.UseSetting("http_port", Setup.PORT).UseEnvironment("Testing");

            builder.ConfigureServices(services =>
            {
                services.AddScoped<ILogin<Admin>, AdminServiceMock>();
            });
        });
    }
}