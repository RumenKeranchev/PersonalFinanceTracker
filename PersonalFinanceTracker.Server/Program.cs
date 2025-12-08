using Microsoft.EntityFrameworkCore;
using PersonalFinanceTracker.Server.Infrastructure;
using PersonalFinanceTracker.Server.Modules.Finance.Endpoints;
using PersonalFinanceTracker.Server.Modules.Reporting.Endpoints;
using PersonalFinanceTracker.Server.Modules.Users.Endpoints;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddOpenApi();

var app = builder.Build();

app
    .MapFinanceModule()
    .MapReportingModule()
    .MapUsersModel();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.Run();
