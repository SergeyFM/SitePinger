using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SitePinger.Ping;

namespace SitePinger.ConsoleApp;

internal class Program {
    static void Main(string[] args) {

        // Read configuration from config.json file
        IConfigurationRoot configuration = new ConfigurationBuilder()
            //.SetBasePath(Path.GetDirectoryName(Assembly.GetExecutingAssembly()?.Location) ?? "")
            .AddJsonFile("config.json")
            .Build();

        // Add DI
        IServiceCollection services = new ServiceCollection();

        // Add configuration
        services.AddSingleton<IConfiguration>(configuration);

        // Add options
        services.AddOptions<PingSettings>()
            .Bind(configuration.GetSection("PingSettings"))
            .ValidateOnStart();

        // Add services
        services.AddSingleton<IPingService, PingService.PingService>();

        IServiceProvider serviceProvider = services.BuildServiceProvider();

        // Do stuff
        RunLoop(serviceProvider);

    }

    static async void RunLoop(
            IServiceProvider serviceProvider
        ) {

        PingSettings pingSettings = serviceProvider.GetRequiredService<IOptions<PingSettings>>().Value;
        Console.WriteLine(pingSettings);

        string address = pingSettings.WebServer;
        int port = pingSettings.Port;
        int sleep = pingSettings.IntervalSec;
        int timeout = pingSettings.TimeoutSec;

        IPingService pingService = serviceProvider.GetRequiredService<IPingService>();
        while (true) {
            Task<long?> reqTask = pingService.MeasureRequestTimeAsync(address, port, timeout);
            reqTask.Wait();
            Thread.Sleep(sleep * 1000 + new Random().Next(1, 6) * 100);
        }
    }
}
