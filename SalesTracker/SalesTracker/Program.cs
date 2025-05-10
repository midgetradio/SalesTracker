using ElmahCore.Mvc;
using ElmahCore.Sql;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using SalesTracker.Data;
using SalesTracker.Utility;

var builder = WebApplication.CreateBuilder(args);

var settings = builder.Configuration.GetSection("Settings").Get<Settings>();

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

var sqlId = Environment.GetEnvironmentVariable("SQL_UID");
var sqlPwd = Environment.GetEnvironmentVariable("SQL_PWD");

if (String.IsNullOrEmpty(connectionString))
{
    Console.WriteLine("Could not load connection string.");
    return;
}

if (String.IsNullOrEmpty(sqlPwd) || String.IsNullOrEmpty(sqlId))
{
    Console.WriteLine("Could not load sql user id or password");
    return;
}

connectionString = connectionString.Replace("{SQL_UID}", sqlId);
connectionString = connectionString.Replace("{SQL_PWD}", sqlPwd);

builder.Services.AddDbContext<SalesTrackerDBContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddElmah<SqlErrorLog>(elmahOptions =>
{
    elmahOptions.ConnectionString = connectionString;
    elmahOptions.Path = "list_errors";
});

builder.Services.AddSingleton<PageHitsTracker>();

builder.Services.AddControllersWithViews();

builder.Services.AddMvc();

var app = builder.Build();

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

if (app.Environment.IsProduction())
{
    app.UseHttpsRedirection();
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseElmah();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
