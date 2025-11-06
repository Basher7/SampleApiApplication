using Domain.Entities.Jwe;
using FluentValidation;
using SampleApiApplication;
using Serilog;
using System.Runtime.InteropServices;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

Log.Information("starting server.");
var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, loggerConfiguration) =>
{
    loggerConfiguration.WriteTo.Console();
    loggerConfiguration.ReadFrom.Configuration(context.Configuration);
});

builder.WebHost
    .CaptureStartupErrors(true)
    .UseSetting(WebHostDefaults.DetailedErrorsKey, "true");
IServiceCollection services = builder.Services;

bool isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

var environementName = builder.Environment.EnvironmentName;
builder.Configuration.AddJsonFile($"appsettings.{environementName}.json", optional: true, reloadOnChange: true);

Startup.ConfigureServices(services, isWindows);

// By service way value binded after all services regester. Use below mechanism for static model bind
builder.Configuration.GetSection("AuthKeys").Get<AuthKeys>();

// Add Validation settings
ValidatorOptions.Global.DefaultClassLevelCascadeMode = CascadeMode.Stop;
ValidatorOptions.Global.DefaultRuleLevelCascadeMode = CascadeMode.Stop;

var app = builder.Build();
Startup.ConfigureMethod(app, isWindows);

app.UseSerilogRequestLogging();

app.Run();
