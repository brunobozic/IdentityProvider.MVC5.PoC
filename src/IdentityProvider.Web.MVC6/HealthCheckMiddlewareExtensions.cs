using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

public static class HealthCheckMiddlewareExtensions
{
    public static IApplicationBuilder UseCustomHealthChecks(this IApplicationBuilder app)
    {
        // General health check endpoint
        app.UseHealthChecks("/hc", new HealthCheckOptions
        {
            Predicate = _ => true, // Run all registered health checks
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });

        // Liveness probe endpoint
        app.UseHealthChecks("/liveness", new HealthCheckOptions
        {
            Predicate = r => r.Name.Contains("self"), // Filter for specific health checks
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });

        return app;
    }
}
