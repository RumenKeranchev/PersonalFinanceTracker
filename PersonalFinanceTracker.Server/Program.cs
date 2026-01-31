using Asp.Versioning;
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

    builder.Services.AddCors(options =>
        options.AddPolicy("DevCors",
            policy => policy
                .WithOrigins("https://localhost:56733")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials()));

    #region Auth

    builder.Services
        .AddIdentity<AppUser, IdentityRole<Guid>>()
        .AddEntityFrameworkStores<AppDbContext>()
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

            opt.Events = new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    // If the token is stored as an HttpOnly cookie named "accessToken", pick it up
                    string? cookieToken = context.Request.Cookies["accessToken"];
                    if (!string.IsNullOrEmpty(cookieToken))
                    {
                        context.Token = cookieToken;
                    }

                    return Task.CompletedTask;
                }
            };
        });

    builder.Services.AddAuthorization();

    #endregion

    #region Versioning

    builder.Services
        .AddApiVersioning(opt =>
{
    opt.DefaultApiVersion = new(1);
    opt.ReportApiVersions = true;

    // this will not make /api/user/... acceptable! if the group has ../v1/.. in the path it must be included!
    // HOWEVER, this will default to v1 if there are more versions and the X-Api-Version header is missing
    // and the route doesn't include ../v1/.. in it
    opt.AssumeDefaultVersionWhenUnspecified = true;

    opt.ApiVersionReader = ApiVersionReader.Combine(
        new UrlSegmentApiVersionReader(),
        new HeaderApiVersionReader("X-Api-Version"));
})
        .AddApiExplorer(opt =>
{
    opt.GroupNameFormat = "'v'VVV";
    opt.SubstituteApiVersionInUrl = true;
});

    #endregion

    #region Swagger

    builder.Services.AddOpenApi();
    builder.Services.AddOpenApiDocument();

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(opt => opt.CustomSchemaIds(type => type.FullName ?? type.Name));
    #endregion

    #region Register services

    builder.Services.AddHttpContextAccessor();
    builder.Services.AddScoped<ICurrentUser, CurrentUser>();
    builder.Services.RegisterFinanceServices();
    builder.Services.RegisterUsersServices();

    #endregion

    var app = builder.Build();

    bool needsSeed = await Seeder.SeedUsersAsync(app.Services);

    if (needsSeed)
    {
        var db = app.Services.CreateScope().ServiceProvider.GetRequiredService<AppDbContext>();
        Seeder.FakeData();

        db.Categories.AddRange(Seeder.Categories);
        db.Transactions.AddRange(Seeder.Transactions);
        db.Budgets.AddRange(Seeder.Budgets);

        await db.SaveChangesAsync();
    }

    #region Global exception handling

    app.UseExceptionHandler();

    #endregion

    app.UseCors("DevCors");
    app.UseOpenApi();
    app.UseAuthentication();
    app.UseAuthorization();

    #region Map endpoints

    var versionSet = app.NewApiVersionSet()
        .HasApiVersion(new(1))
        .ReportApiVersions()
        .Build();

    app.MapFinanceModule(versionSet);
    app.MapUsersModule(versionSet);
    app.MapReportingModule(versionSet);

    #endregion

    if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi();

        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.Run();

}
finally
{
    LogManager.Shutdown();

}