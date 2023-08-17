using ElmahCore.Mvc;
using ElmahCore.Sql;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.EnvironmentVariables;
using SalesTracker.Data;
using SalesTracker.Utility;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
try
{
    connectionString = EnvironmentVariableReplacer.Replace(connectionString);
}
catch (Exception e)
{
    Console.WriteLine(e.Message);
    Console.WriteLine("Connection String: " + connectionString);
}

builder.Services.AddDbContext<SalesTrackerDBContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddElmah<SqlErrorLog>(elmahOptions =>
{
    elmahOptions.ConnectionString = connectionString;
    elmahOptions.Path = "list_errors";
});

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
