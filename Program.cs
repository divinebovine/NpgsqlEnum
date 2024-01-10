using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Npgsql;

namespace npgsqlEnum;
public class Program
{
    private static void Main(string[] args) =>
         CreateHostBuilder(args)
            .Build()
            .Run();

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(
                webBuilder => webBuilder.UseStartup<Startup>());

}

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services
                .AddLogging()
                .AddHostedService<MyService>()
                .AddDbContext<MyContext>(options =>
                {
                    var connectionString = "host=localhost; port=9876; database=test; username=postgres; password=postgres; IncludeErrorDetail=true;";
                    var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);
                    dataSourceBuilder.MapEnum<Color>();
                    var dataSource = dataSourceBuilder.Build();

                    options
                        .UseNpgsql(dataSource)
                        .UseSnakeCaseNamingConvention();
                });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
    }
}
