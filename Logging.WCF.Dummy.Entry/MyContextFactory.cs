using Logging.WCF.Repository.EF.EFDataContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Logging.WCF.Dummy.Entry
{
    public class MyContextContextFactory : IDesignTimeDbContextFactory<MyAppDbContext>
    {
        public MyAppDbContext CreateDbContext(string[] args)
        {
            var envName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            if (string.IsNullOrEmpty(envName))
            {
#if DEBUG
                envName = "Development";
#endif
#if !DEBUG
                envName = "Production";
#endif
            }

            Log.Information("MyContextContextFactory Environment: " + envName);

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile($"appsettings.{envName}.json", true)
                .Build();

            // Here we create the DbContextOptionsBuilder manually.        
            var builder = new DbContextOptionsBuilder<MyAppDbContext>();

            // Build connection string. This requires that you have a connectionstring in the appsettings.json
            var connectionString = configuration.GetConnectionString("DatabaseLogger");

            Log.Information("MyContextContextFactory GetConnectionString: " + connectionString);

            // builder.UseSqlite(connectionString);
            // builder.UseNpgsql(connectionString);
            builder.UseSqlServer(connectionString);

            // Create our DbContext.
            return new MyAppDbContext(builder.Options, connectionString);
        }
    }
}
