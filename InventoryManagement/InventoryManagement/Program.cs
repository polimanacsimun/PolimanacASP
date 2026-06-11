using InventoryManagement.DAL;
using InventoryManagement.DAL.Repositories;
using InventoryManagement.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// MVC + API controllers
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<InventoryManagementDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("InventoryManagementDbContext")));

// ASP.NET Core Identity
builder.Services
    .AddIdentity<AppUser, IdentityRole>(options =>
    {
        options.SignIn.RequireConfirmedAccount = false;

        // Malo jednostavnije lozinke za laboratorij i testiranje.
        options.Password.RequiredLength = 6;
        options.Password.RequireDigit = false;
        options.Password.RequireLowercase = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireNonAlphanumeric = false;

        options.User.RequireUniqueEmail = true;
    })
    .AddEntityFrameworkStores<InventoryManagementDbContext>()
    .AddDefaultTokenProviders();

// Existing EF repositories
builder.Services.AddScoped<ProductEfRepository>();
builder.Services.AddScoped<SupplierEfRepository>();
builder.Services.AddScoped<CategoryEfRepository>();
builder.Services.AddScoped<WarehouseEfRepository>();
builder.Services.AddScoped<UserEfRepository>();
builder.Services.AddScoped<OrderEfRepository>();
builder.Services.AddScoped<InventoryItemEfRepository>();
builder.Services.AddScoped<OrderItemEfRepository>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllers();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();