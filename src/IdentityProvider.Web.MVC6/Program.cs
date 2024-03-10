using Autofac;
using Autofac.Extensions.DependencyInjection;
using IdentityProvider.Repository.EFCore;
using IdentityProvider.Repository.EFCore.EFDataContext;
using IdentityProvider.ServiceLayer.Services;
using IdentityProvider.ServiceLayer.Services.OperationsService;
using IdentityProvider.Web.MVC6;
using IdentityProvider.Web.MVC6.Middleware;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Module.CrossCutting;
using Module.CrossCutting.Cookies;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Serilog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Mime;
ILifetimeScope AutofacContainer;

var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    Args = args,
    ApplicationName = typeof(Program).Assembly.FullName,
    ContentRootPath = Directory.GetCurrentDirectory(),
    EnvironmentName = Environments.Development
});

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Configure Serilog with the extension method
builder.Host.UseSerilogConfiguration();

// Configure MediatR with the extension method
builder.Services.AddMediatRConfiguration();

// Configure data services with the extension method
builder.Services.ConfigureDataServices(builder.Configuration);

// Assuming 'connectionString' is defined or fetched as shown previously
builder.Services.AddCustomIdentityConfiguration(connectionString);

// Add custom health checks with the extension method
builder.Services.AddCustomHealthChecks(connectionString);


builder.Services.RegisterRepositories();
builder.Services.RegisterServices();

// Add other services
builder.Services.AddProblemDetails();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddSingleton<IMyConfigurationValues, MyConfigurationValues>();
builder.Services.AddSingleton<ICookieStorageService, CookieStorageService>();
builder.Services.AddTransient<IDomainEventsDispatcher, IntegrationEventDispatcher>();
//builder.Services.AddTransient<IMailService, MailService>();
//builder.Services.AddTransient<IMailkitSendEmailJob, MailkitSendEmailJob>();
builder.Services.AddScoped<IOperationService, OperationService>();


#region MVC and razor and authorization

// Configure MVC
builder.Services.AddControllersWithViews()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.Converters.Add(new StringEnumConverter(new CamelCaseNamingStrategy()));
        options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
        options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
    })
    .ConfigureApiBehaviorOptions(options =>
    {
        options.InvalidModelStateResponseFactory = context =>
        {
            var result = new BadRequestObjectResult(context.ModelState);
            result.ContentTypes.Add(MediaTypeNames.Application.Json);
            result.ContentTypes.Add(MediaTypeNames.Application.Xml);
            return result;
        };
        options.SuppressConsumesConstraintForFormFileParameters = false;
        options.SuppressInferBindingSourcesForParameters = false;
        options.SuppressModelStateInvalidFilter = true;
        options.SuppressMapClientErrors = true;
        options.ClientErrorMapping[404].Link = "https://httpstatuses.com/404";
    });

// Configure Razor Pages and Authorization
builder.Services.AddRazorPages();
builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
});

#endregion MVC and razor and authorization

// Add AutoMapper with the extension method
builder.Services.AddApplicationAutoMapper();

#region Autofac
AutofacContainer = Bootstrap.BuildContainer(connectionString, builder.Services);
var serviceProvider = new AutofacServiceProvider(AutofacContainer);
#endregion Autofac

// ========================================================================================================
// ========================================================================================================
// ========================================================================================================
var app = builder.Build();
// ========================================================================================================
// ========================================================================================================
// ========================================================================================================

// Configure logging
IHostApplicationLifetime lifetime = app.Lifetime;
IWebHostEnvironment env = app.Environment;
IMyConfigurationValues service = app.Services.GetRequiredService<IMyConfigurationValues>();
Microsoft.Extensions.Logging.ILogger logger = app.Logger;
lifetime.ApplicationStarted.Register(() =>
    logger.LogInformation(
        $"The application {env.ApplicationName} started" +
        $" with injected {service}"));


#region Migrations

// Apply migrations and seed data
Log.Warning("Applying migrations ({ApplicationContext})...", typeof(Program).Namespace);
var configuration = app.Services.GetRequiredService<IConfiguration>();
var testUserPw = configuration["SeedUserPW"];
if (string.IsNullOrEmpty(testUserPw)) { testUserPw = "theSecret101!"; }

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var dbContext = services.GetRequiredService<AppDbContext>();
        dbContext.Database.Migrate();
        await SeedData.Initialize(services, testUserPw);
    }
    catch (Exception ex)
    {
        var mah_logger = services.GetRequiredService<ILogger<Program>>();
        mah_logger.LogError(ex, "An error occurred while migrating or seeding the database.");
    }
}
Log.Warning("Migrations ({ApplicationContext}) applied...", typeof(Program).Namespace);

#endregion Migrations

// Configure HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    //  app.UseMigrationsEndPoint();
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<UnhandledExceptionMiddleware>();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.UseCustomHealthChecks(); // Apply the health checks middleware configurations

// Run the application
try
{
    app.Run();
    return 0;
}
catch (Exception ex)
{
    Log.Error(ex.Message, typeof(Program).Namespace);
    return 1;
}
finally { await Log.CloseAndFlushAsync(); }

#region Error catcher
class BadRequestEventListener : IObserver<KeyValuePair<string, object>>, IDisposable
{
    private readonly IDisposable _subscription;
    private readonly Action<IBadRequestExceptionFeature> _callback;

    public BadRequestEventListener(DiagnosticListener diagnosticListener,
                                   Action<IBadRequestExceptionFeature> callback)
    {
        _subscription = diagnosticListener.Subscribe(this!, IsEnabled);
        _callback = callback;
    }

    private static readonly Predicate<string> IsEnabled = (provider) => provider switch
    {
        "Microsoft.AspNetCore.Server.Kestrel.BadRequest" => true,
        _ => false
    };

    public void OnNext(KeyValuePair<string, object> pair)
    {
        if (pair.Value is IFeatureCollection featureCollection)
        {
            var badRequestFeature = featureCollection.Get<IBadRequestExceptionFeature>();

            if (badRequestFeature is not null)
            {
                _callback(badRequestFeature);
            }
        }
    }

    public void OnError(Exception error) { }
    public void OnCompleted() { }
    public virtual void Dispose() => _subscription.Dispose();
}
#endregion Error catcher