using Microsoft.EntityFrameworkCore;
using NLog;
using NLog.Web;
using PersonalFinanceTracker.Server.Middleware;
using PersonalFinanceTracker.Server.Modules.Finance.Endpoints;
using PersonalFinanceTracker.Server.Modules.Reporting.Endpoints;
using PersonalFinanceTracker.Server.Modules.Users.Endpoints;

var logger = LogManager.Setup()
    .LoadConfigurationFromFile("nlog.config")
    .GetCurrentClassLogger();

try
{

    var builder = WebApplication.CreateBuilder(args);

    #region Global exception handling

    builder.Logging.ClearProviders();
    builder.Host.UseNLog();

    builder.Services.AddExceptionHandler<UnexpectedExeptionHandler>();
    builder.Services.AddProblemDetails();

    #endregion

    builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

    builder.Services.AddOpenApi();

    builder.Services.RegisterFinanceServices();

    var app = builder.Build();

    #region Global exception handling

    app.UseExceptionHandler();

    #endregion

    app
        .MapFinanceModule()
        .MapReportingModule()
        .MapUsersModule();

    if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi();
    }

    app.Run();

}
finally
{
    LogManager.Shutdown();

}