using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace npgsqlEnum;
public class MyService : BackgroundService
{
    private readonly ILogger logger;
    private readonly IServiceScopeFactory serviceScopeFactory;
    private readonly IHostApplicationLifetime hostApplicationLifetime;
    private readonly Random random;
    private readonly Color[] colorWheel = new Color[] { Color.Red, Color.Green, Color.Blue, Color.Yellow, Color.Magenta, Color.Orange };

    public MyService(ILogger<MyService> logger, IServiceScopeFactory serviceScopeFactory, IHostApplicationLifetime hostApplicationLifetime)
    {
        this.logger = logger;
        this.serviceScopeFactory = serviceScopeFactory;
        this.hostApplicationLifetime = hostApplicationLifetime;
        random = new Random();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var scope = serviceScopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<MyContext>();

        for (var i = 0; i < 10; i++)
        { context.Entries.Add(new Entry { Color = colorWheel[random.Next(1, colorWheel.Length)] }); }

        await context.SaveChangesAsync(stoppingToken);

        var fetchedEntries = await context.Entries
            .OrderBy(x => x.Id)
            .ToListAsync(stoppingToken);

        var sb = new StringBuilder();
        foreach (var entry in fetchedEntries)
        {
            sb.AppendLine($"Entry ID: {entry.Id} Color: {Enum.GetName(typeof(Color), entry.Color)}");
        }
        logger.LogInformation(sb.ToString());

        hostApplicationLifetime.StopApplication();
    }
}
