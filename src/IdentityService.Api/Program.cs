using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;
using App.Metrics.AspNetCore;
using App.Metrics.Formatters.Prometheus;
using Dapr.Client;
using Dapr.Extensions.Configuration;

namespace IdentityService.Api
{
    public class Program
    {
        public static int Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration().MinimumLevel
                .Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
                .MinimumLevel.Override("System", LogEventLevel.Warning)
                .MinimumLevel.Override(
                    "Microsoft.AspNetCore.Authentication",
                    LogEventLevel.Information
                )
                .Enrich.FromLogContext()
                .WriteTo.Console(
                    outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}",
                    theme: AnsiConsoleTheme.Code
                )
                .CreateLogger();

            var host = CreateHostBuilder(args).Build();

            Log.Information("Starting host...");
            host.Run();
            return 0;
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(
                    config =>
                    {
                        //var daprClient = new DaprClientBuilder().Build();
                        //var secretDescriptors = new List<DaprSecretDescriptor>
                        //{
                        //    new DaprSecretDescriptor("connectionStrings")
                        //};
                        //config.AddDaprSecretStore("app-local-secret-store", secretDescriptors, daprClient);

                        //// Get the initial value from the configuration component.
                        //config.AddDaprConfigurationStore(
                        //    "appsettingsconfigstore",
                        //    new List<string>() { "withdrawVersion" },
                        //    daprClient,
                        //    TimeSpan.FromSeconds(20)
                        //);

                        // Watch the keys in the configuration component and update it in local configurations.
                        //config.AddStreamingDaprConfigurationStore(
                        //    "redisconfig",
                        //    new List<string>() { "withdrawVersion", "source" },
                        //    daprClient,
                        //    TimeSpan.FromSeconds(20)
                        //);
                    }
                )
                .UseMetricsWebTracking()
                .UseMetrics(
                    option =>
                    {
                        option.EndpointOptions = endpointOption =>
                        {
                            endpointOption.MetricsTextEndpointOutputFormatter =
                                new MetricsPrometheusTextOutputFormatter();
                            endpointOption.MetricsEndpointOutputFormatter =
                                new MetricsPrometheusProtobufOutputFormatter();
                            endpointOption.EnvironmentInfoEndpointEnabled = false;
                        };
                    }
                )
                .ConfigureWebHostDefaults(
                    webBuilder =>
                    {
                        webBuilder.UseStartup<Startup>();
                    }
                );
    }
}
