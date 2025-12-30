using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NLog;
using NLog.Web;
using PersonalFinanceTracker.Server.Middleware;
using PersonalFinanceTracker.Server.Modules.Finance.Endpoints;
using PersonalFinanceTracker.Server.Modules.Reporting.Endpoints;
using PersonalFinanceTracker.Server.Modules.Users.Domain;
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

    builder.Services.AddIdentity<AppUser, IdentityRole>()
        .AddEntityFrameworkStores<AppDbContext>()
        .AddSignInManager()
        .AddUserManager<AppUser>()
        .AddRoles<IdentityRole>()
        .AddDefaultTokenProviders();

    builder.Services.AddAuthentication();
    builder.Services.AddAuthorization();

    builder.Services.AddOpenApi();

    #region Register services

    builder.Services.RegisterFinanceServices();
    builder.Services.RegisterUsersServices();

    #endregion

    var app = builder.Build();

    #region Global exception handling

    app.UseExceptionHandler();

    #endregion

    app.UseAuthentication();
    app.UseAuthorization();

    #region Map endpoints

    app.MapFinanceModule();
    app.MapUsersModule();
    app.MapReportingModule();

    #endregion

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