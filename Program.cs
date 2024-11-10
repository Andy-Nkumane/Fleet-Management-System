using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using FleetManagementSystem.Data;
using FleetManagementSystem.Models;

var builder = WebApplication.CreateBuilder(args);

if (builder.Environment.IsDevelopment())
{
builder.Services.AddDbContext<FleetManagementSystemContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("FleetManagementSystemContext") ?? throw new InvalidOperationException("Connection string 'FleetManagementSystemContext' not found.")));
}
else
{
    builder.Services.AddDbContext<FleetManagementSystemContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("ProductionFleetManagementSystemContext")));
}

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

using (var scope = app.Services.CreateAsyncScope())
{
    var services = scope.ServiceProvider;

    SeedData.Initialize(services);
}

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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
