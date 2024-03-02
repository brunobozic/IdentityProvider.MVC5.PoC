using AutoMapper;
using HealthChecks.UI.Client;
using IdentityProvider.Repository.EFCore.Domain.Account;
using IdentityProvider.Repository.EFCore.Domain.Roles;
using IdentityProvider.Repository.EFCore.EFDataContext;
using IdentityProvider.Web.MVC6.Controllers;
using IdentityProvider.Web.MVC6.Middleware;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Module.CrossCutting;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Mime;

var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    Args = args,
    ApplicationName = typeof(Program).Assembly.FullName,
    ContentRootPath = Directory.GetCurrentDirectory(),
    EnvironmentName = Environments.Development
});

// Configure Serilog
builder.Host.UseSerilog();
builder.Logging.AddSerilog();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Add services to the container
builder.Services.AddLogging(loggingBuilder =>
{
    loggingBuilder.ClearProviders();
    loggingBuilder.AddSerilog(dispose: true);
});

// Configure Mediatr
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));
builder.Services.AddTransient<IMediator, Mediator>();
builder.Services.AddTransient<IMediator, NoMediator>();

// Configure DbContext
builder.Services.AddDbContext<AppDbContext>((serviceProvider, options) =>
{
    options.UseSqlServer(connectionString, sqlOptions =>
    {
        sqlOptions.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(30),
            errorNumbersToAdd: null
        );
    });
});

//builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
//    .AddEntityFrameworkStores<AppDbContext>();

builder.Services.AddScoped<UserManager<ApplicationUser>>();
builder.Services.AddScoped<RoleManager<AppRole>>();
builder.Services.AddScoped<IUserStore<ApplicationUser>, UserStore<ApplicationUser, AppRole, AppDbContext, Guid>>();
builder.Services.AddScoped<IRoleStore<AppRole>, RoleStore<AppRole, AppDbContext, Guid>>();

builder.Services.AddIdentity<ApplicationUser, AppRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddRoleManager<RoleManager<AppRole>>()
    .AddUserManager<UserManager<ApplicationUser>>()
    .AddDefaultTokenProviders();

builder.Services.Configure<IdentityOptions>(options =>
{
    // Password settings
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;

    // Lockout settings
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    // User settings
    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = false;
});

// Add other services
builder.Services.AddProblemDetails();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<IMyConfigurationValues, MyConfigurationValues>();

builder.Services.AddHealthChecks()
    .AddSqlServer(connectionString)
    .AddCheck("self", () => HealthCheckResult.Healthy(), tags: new[] { "self" });

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

// Configure AutoMapper
var config = new MapperConfiguration(cfg => { cfg.AddMaps("IdentityProvider.ServiceLayer"); });
var mapper = config.CreateMapper();
builder.Services.AddSingleton(mapper);

var app = builder.Build();

// Configure logging
IHostApplicationLifetime lifetime = app.Lifetime;
IWebHostEnvironment env = app.Environment;
IMyConfigurationValues service = app.Services.GetRequiredService<IMyConfigurationValues>();
Microsoft.Extensions.Logging.ILogger logger = app.Logger;
lifetime.ApplicationStarted.Register(() =>
    logger.LogInformation(
        $"The application {env.ApplicationName} started" +
        $" with injected {service}"));

Log.Logger = CreateSerilogLogger(app.Configuration);

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
        SeedData.Initialize(dbContext, scope, testUserPw).Wait();
    }
    catch (Exception ex)
    {
        var mah_logger = services.GetRequiredService<ILogger<Program>>();
        mah_logger.LogError(ex, "An error occurred while migrating or seeding the database.");
    }
}
Log.Warning("Migrations ({ApplicationContext}) applied...", typeof(Program).Namespace);

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

// Configure health checks
app.UseHealthChecks("/hc", new HealthCheckOptions
{
    Predicate = _ => true,
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.UseHealthChecks("/liveness", new HealthCheckOptions
{
    Predicate = r => r.Name.Contains("self")
});

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

static Serilog.ILogger CreateSerilogLogger(IConfiguration configuration)
{
    var appInstanceName = configuration["InstanceName"];
    var environment = configuration["Environment"];

    return new LoggerConfiguration()
        .ReadFrom.Configuration(configuration)
        .Enrich.WithProperty("Application", appInstanceName)
        .Enrich.WithProperty("Environment", environment)
        .Enrich.FromLogContext()
        .Enrich.WithAssemblyVersion()
        .Enrich.WithEnvironmentName()
        .Enrich.WithProcessName()
        .Enrich.WithEnvironmentUserName()
        .Enrich.WithEnvironment(environment)
        .Enrich.WithProperty("DebuggerAttached", Debugger.IsAttached)
        .WriteTo.Console(theme: AnsiConsoleTheme.Code,
            outputTemplate:
            "{Timestamp:HH:mm} [{Level}] [{Address}] {Site}: {Message} || Application: [{Application}], Machine: [{MachineName}], User: [{EnvironmentUserName}], CorrelationId: [{CorrelationId}], DebuggerAttached: [{DebuggerAttached}] {NewLine}")
        .WriteTo.File(appInstanceName + ".log", rollingInterval: RollingInterval.Day, retainedFileCountLimit: null)
        .CreateLogger();
}

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
