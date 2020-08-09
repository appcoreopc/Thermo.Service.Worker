using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Service.MessageBusServiceProvider.AzBlob;
using Service.MessageBusServiceProvider.CheckPointing;
using Service.ThermoProcessWorker.AppBusinessLogic;

namespace Service.ThermoProcessWorker
{
    public class Program
    {
        public IConfigurationRoot Configuration { get; set; }

        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .UseWindowsService()
            .ConfigureServices((hostContext, services) =>
            {
                services.AddLogging();             
                services.AddSingleton<IBlobClientProvider, BlobClientProvider>();
                services.AddSingleton<IChannelMessageSender, ChannelMessageSender>();
                services.AddSingleton<ICheckPointLogger, CheckPointLogger>();
                services.AddSingleton<IThermoDataLogic, ThermoDataLogic>();
                services.AddHostedService<BackgroundRestWorkerService>();
            }).ConfigureLogging(
            loggingBuilder =>
            {
                var configuration = new ConfigurationBuilder()
                   .AddJsonFile("appsettings.json")
                   .Build();
                var logger = new LoggerConfiguration()
                    .ReadFrom.Configuration(configuration)
                    .CreateLogger();
                loggingBuilder.AddSerilog(logger, dispose: true);
        }
      );
    }
}
