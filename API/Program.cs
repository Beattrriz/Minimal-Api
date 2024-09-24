using MinimalAPI;
using MinimalAPI.Api;

IHostBuilder CreateHostBuilder(string[] args){
    return Host.CreateDefaultBuilder(args).ConfigureWebHostDefaults(webBuilder =>
    {
        webBuilder.UseStartup<Startup>();
    });
}

CreateHostBuilder(args).Build().Run();