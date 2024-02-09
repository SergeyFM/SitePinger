using System.Net;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SitePinger.Ping;

namespace SitePinger.ConsoleApp;

internal class Program {
    static void Main(string[] args) {


        // Read configuration from config.json file
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Path.GetDirectoryName(Assembly.GetExecutingAssembly()?.Location) ?? "")
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
        

        IServiceProvider serviceProvider = services.BuildServiceProvider();

        var pingSettings = serviceProvider.GetRequiredService<IOptions<PingSettings>>().Value;

        Console.WriteLine(pingSettings);

        string address = pingSettings.WebServer;
        int port = pingSettings.Port;

        // Assuming measureRequestTimeTask is a static method in a module or a static class
        var timeTakenOption = PingService.measureRequestTimeTask(address, port);
        timeTakenOption.Wait();

        
        Console.WriteLine(timeTakenOption.Result);


    }
}
