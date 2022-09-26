using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;
using App.Metrics.AspNetCore;
using App.Metrics.Formatters.Prometheus;

namespace IdentityService.Api {

    public class Program
    {
        public static int Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
               .MinimumLevel.Debug()
               .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
               .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
               .MinimumLevel.Override("System", LogEventLevel.Warning)
               .MinimumLevel.Override("Microsoft.AspNetCore.Authentication", LogEventLevel.Information)
               .Enrich.FromLogContext()
               .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}", theme: AnsiConsoleTheme.Code)
               .CreateLogger();

            var host = CreateHostBuilder(args).Build();
          

            Log.Information("Starting host...");
            host.Run();
            return 0;
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .UseMetricsWebTracking().UseMetrics(option =>
            {
                option.EndpointOptions = endpointOption =>
                {
                    endpointOption.MetricsTextEndpointOutputFormatter = new MetricsPrometheusTextOutputFormatter();
                    endpointOption.MetricsEndpointOutputFormatter = new MetricsPrometheusProtobufOutputFormatter();
                    endpointOption.EnvironmentInfoEndpointEnabled = false;
                };
            })
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
    }

}