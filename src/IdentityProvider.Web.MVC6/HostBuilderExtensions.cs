using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;
using System.Diagnostics;

namespace IdentityProvider.Web.MVC6
{
    public static class HostBuilderExtensions
    {
        public static IHostBuilder UseSerilogConfiguration(this IHostBuilder hostBuilder)
        {
            hostBuilder.UseSerilog((context, services, loggerConfiguration) =>
            {
                var appInstanceName = context.Configuration["InstanceName"];
                var environment = context.HostingEnvironment.EnvironmentName;

                loggerConfiguration
                    .ReadFrom.Configuration(context.Configuration)
                    .Enrich.FromLogContext()
                    .Enrich.WithProperty("Application", appInstanceName)
                    .Enrich.WithProperty("Environment", environment)
                    .Enrich.WithAssemblyVersion()
                    .Enrich.WithEnvironmentName()
                    .Enrich.WithProcessName()
                    .Enrich.WithEnvironmentUserName()
                    .Enrich.WithEnvironment(environment)
                    .Enrich.WithProperty("DebuggerAttached", Debugger.IsAttached)
                    .WriteTo.Console(theme: AnsiConsoleTheme.Code, outputTemplate:
                        "{Timestamp:HH:mm} [{Level}] [{Address}] {Site}: {Message} || Application: [{Application}], Machine: [{MachineName}], User: [{EnvironmentUserName}], CorrelationId: [{CorrelationId}], DebuggerAttached: [{DebuggerAttached}] {NewLine}")
                    .WriteTo.File($"{appInstanceName}.log", rollingInterval: RollingInterval.Day, retainedFileCountLimit: null);
            });

            return hostBuilder;
        }
    }
}
