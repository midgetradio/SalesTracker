using ElmahCore.Mvc;
using ElmahCore.Sql;
using Microsoft.EntityFrameworkCore;
using SalesTracker.Data;
using SalesTracker.Utility;

var builder = WebApplication.CreateBuilder(args);

var settings = builder.Configuration.GetSection("Settings").Get<Settings>();

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
Console.WriteLine("CONNECTION STRING BEFORE REPLACEMENT: " + connectionString);
connectionString = EnvironmentVariableReplacer.Replace(connectionString, settings);
Console.WriteLine("CONNECTION STRING AFTER REPLACEMENT: " + connectionString);

builder.Services.AddDbContext<SalesTrackerDBContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddElmah<SqlErrorLog>(elmahOptions =>
{
    elmahOptions.ConnectionString = connectionString;
    elmahOptions.Path = "list_errors";
});

builder.Services.AddSingleton<PageHitsTracker>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseElmah();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
