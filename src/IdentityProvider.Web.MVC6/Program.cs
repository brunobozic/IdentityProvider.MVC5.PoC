﻿using AutoMapper;
using HealthChecks.UI.Client;
using IdentityProvider.Repository.EFCore.Domain.Account;
using IdentityProvider.Repository.EFCore.Domain.Roles;
using IdentityProvider.Repository.EFCore.EFDataContext;
using IdentityProvider.Web.MVC6.Controllers;
using IdentityProvider.Web.MVC6.Identity;
using IdentityProvider.Web.MVC6.Middleware;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
    EnvironmentName = Environments.Staging
});


builder.Host.UseSerilog();
builder.Logging.AddSerilog();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Add services to the container.
builder.Services.AddLogging();
#region Mediatr

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

builder.Services.AddTransient<IMediator, Mediator>();
builder.Services.AddTransient<IMediator, NoMediator>();

#endregion Mediatr
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddProblemDetails();
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddHealthChecks();

builder.Services.AddHttpContextAccessor();

builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddIdentity<ApplicationUser, AppRole>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

builder.Services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>, AdditionalUserClaimsPrincipalFactory>();

builder.Services.Configure<IdentityOptions>(options =>
{
    // Password settings.
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;

    // Lockout settings.
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    // User settings.
    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = false;
});

//builder.Services.ConfigureApplicationCookie(options =>
//{
//    // Cookie settings
//    options.Cookie.HttpOnly = true;
//    options.ExpireTimeSpan = TimeSpan.FromMinutes(5);
//    options.LoginPath = "/Identity/Account/Login";
//    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
//    options.SlidingExpiration = true;
//});
builder.Services.AddControllersWithViews();
//builder.Services.AddControllersWithViews(config =>
//{
//    var policy = new AuthorizationPolicyBuilder()
//        .RequireAuthenticatedUser()
//        .Build();
//    config.Filters.Add(new AuthorizeFilter(policy));
//    config.Filters.Add(new StopWatchActionFilter());
//});

builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
});

builder.Services.AddRazorPages();

//builder.Services.AddHealthChecks().AddCheck("self", () => HealthCheckResult.Healthy());
builder.Services.AddSingleton<IMyConfigurationValues, MyConfigurationValues>();
builder.Services.AddSingleton(typeof(UserManager<ApplicationUser>));
builder.Services.AddSingleton(typeof(RoleManager<AppRole>));
//builder.Services.AddHttpLogging(logging =>
//{
//    // Customize HTTP logging here.
//    logging.LoggingFields = HttpLoggingFields.All;
//    logging.RequestHeaders.Add("My-Request-Header");
//    logging.ResponseHeaders.Add("My-Response-Header");
//    logging.MediaTypeOptions.AddText("application/javascript");
//    logging.RequestBodyLogLimit = 4096;
//    logging.ResponseBodyLogLimit = 4096;
//});

builder.Services.AddMvc(opt =>
{
    // opt.Filters.Add(typeof(ValidateFilterAttribute));
})
//.AddFluentValidation(fv =>
//{
//    fv.RegisterValidatorsFromAssembly(Assembly.Load("StrippedDownSkeleton.Services")); // the assembly that houses the implemented validators
//  //fv.RunDefaultMvcValidationAfterFluentValidationExecutes = false; // dont run MVC validators after having run the fluent ones
//    fv.ImplicitlyValidateChildProperties = true; // fall through and validate all child elements and their child elementes
//})
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

            // TODO: add `using System.Net.Mime;` to resolve MediaTypeNames
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

#region Automapper

var config = new MapperConfiguration(cfg => { cfg.AddMaps("EFModule.Core.Services"); });

var mapper = config.CreateMapper();
// config.AssertConfigurationIsValid();
builder.Services.AddSingleton(mapper);

#endregion Automapper

var app = builder.Build();

IHostApplicationLifetime lifetime = app.Lifetime;
IWebHostEnvironment env = app.Environment;
IMyConfigurationValues service = app.Services.GetRequiredService<IMyConfigurationValues>();
Microsoft.Extensions.Logging.ILogger logger = app.Logger;
lifetime.ApplicationStarted.Register(() =>
    logger.LogInformation(
        $"The application {env.ApplicationName} started" +
        $" with injected {service}"));

Log.Logger = CreateSerilogLogger(app.Configuration);
//var diagnosticSource = app.Services.GetRequiredService<DiagnosticListener>();
//using var badRequestListener = new BadRequestEventListener(diagnosticSource,
//    (badRequestExceptionFeature) =>
//    {
//        app.Logger.LogError(badRequestExceptionFeature.Error, "Bad request received");
//    });

Log.Warning("Applying migrations ({ApplicationContext})...", typeof(Program).Namespace);

var configuration = app.Services.GetRequiredService<IConfiguration>();

// Set password with the Secret Manager tool.
// dotnet myIdentityUser-secrets set SeedUserPW <pw>
var testUserPw = configuration["SeedUserPW"];

using (var scope = app.Services.CreateScope())
{
    var myDbContext = scope.ServiceProvider.GetService<AppDbContext>();

    SeedData.Initialize(myDbContext, scope, testUserPw).Wait();
}

Log.Warning("Migrations ({ApplicationContext}) applied...", typeof(Program).Namespace);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.UseMigrationsEndPoint();
    //app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
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

#region Health check

app.UseHealthChecks("/hc", new HealthCheckOptions
{
    Predicate = _ => true,
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.UseHealthChecks("/liveness", new HealthCheckOptions
{
    Predicate = r => r.Name.Contains("self")
});

#endregion Health check

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

    // var kafkaProducerForLogging = container.Resolve<IKafkaLoggingProducer>();

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
