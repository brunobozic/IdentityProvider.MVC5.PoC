using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

public static class HealthChecksExtensions
{
    public static IServiceCollection AddCustomHealthChecks(this IServiceCollection services, string connectionString)
    {
        services.AddHealthChecks()
            .AddSqlServer(connectionString, name: "SQL Server", tags: new[] { "db", "sql", "sqlserver" })
            .AddCheck("Self", () => HealthCheckResult.Healthy(), tags: new[] { "self" });

        // Consider adding more health checks as needed for your application:
        // .AddUrlGroup(new Uri("https://example.com"), name: "Example URL", tags: new[] { "url" })
        // .AddRedis(redisConnectionString, name: "Redis", tags: new[] { "db", "cache", "redis" })

        return services;
    }
}
