using Api.Models;
using Api.Services;
using Microsoft.EntityFrameworkCore;
using dotenv.net;

var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";

if(environment == "Development")
{
    DotEnv.Load();
} 

var SQL_UID = Environment.GetEnvironmentVariable("SQL_UID");
var SQL_PWD = Environment.GetEnvironmentVariable("SQL_PWD");

if(string.IsNullOrEmpty(SQL_UID) || string.IsNullOrEmpty(SQL_PWD))
{
    throw new InvalidOperationException("SQL_UID or SQL_PWD environment variables are not set. Application cannot start without credentials.");
}

var builder = WebApplication.CreateBuilder(args);

var connectionString =
    builder.Configuration.GetConnectionString("DefaultConnection")
        ?? throw new InvalidOperationException("Connection string"
        + "'DefaultConnection' not found.");

connectionString = connectionString.Replace("{SQL_UID}", SQL_UID)
                                   .Replace("{SQL_PWD}", SQL_PWD);

builder.Services.AddDbContext<SalesTrackerContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddScoped<EditionsService>();
builder.Services.AddScoped<PageHitService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontendClient", policy =>
    {
        policy.WithOrigins("http://127.0.0.1:4200") // No trailing slash!
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

app.UseCors("FrontendClient"); 

app.MapGet("/", () => "Hello World!");

app.MapGet("/editions/dates", async (EditionsService editionsService) =>
    await editionsService.GetLastTenDates());

app.MapGet("/editions/{date}", async (EditionsService editionsService, DateTime date) =>
    await editionsService.GetEditionsByDateAsync(date));

app.MapGet("/editions", async (EditionsService editionsService) =>
    await editionsService.GetEditionsAsync());

app.MapGet("/saletypes", async (EditionsService editionsService) =>
    await editionsService.GetSaleTypesAsync());

app.MapGet("/pagehits", async (PageHitService pageHitService) =>
    await pageHitService.GetPageHitsLastThirty());

app.Run();
