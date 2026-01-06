using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NLog;
using NLog.Web;
using PersonalFinanceTracker.Server.Middleware;
using PersonalFinanceTracker.Server.Modules.Finance.Endpoints;
using PersonalFinanceTracker.Server.Modules.Reporting.Endpoints;
using PersonalFinanceTracker.Server.Modules.Users.Domain;
using PersonalFinanceTracker.Server.Modules.Users.Endpoints;
using System.Text;

var logger = LogManager.Setup()
    .LoadConfigurationFromFile("nlog.config")
    .GetCurrentClassLogger();

try
{

    var builder = WebApplication.CreateBuilder(args);

    #region Global exception handling

    builder.Logging.ClearProviders();
    builder.Host.UseNLog();

    builder.Services.AddExceptionHandler<UnexpectedExceptionHandler>();
    builder.Services.AddProblemDetails();

    #endregion

    builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

    builder.Services.AddIdentity<AppUser, IdentityRole>()
        .AddEntityFrameworkStores<AppDbContext>()
        .AddSignInManager()
        .AddUserManager<UserManager<AppUser>>()
        .AddRoles<IdentityRole>()
        .AddDefaultTokenProviders();

    builder.Services
        .AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(opt =>
        {
            string key = builder.Configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key is not configured.");
            string iss = builder.Configuration["Jwt:Issuer"] ?? throw new InvalidOperationException("JWT Issuer is not configured.");
            string aud = builder.Configuration["Jwt:Audience"] ?? throw new InvalidOperationException("JWT Audience is not configured.");

            opt.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = iss,
                ValidAudience = aud,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
            };
        });
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