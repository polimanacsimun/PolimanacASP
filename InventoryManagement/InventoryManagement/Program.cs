using InventoryManagement.DAL;
using Microsoft.EntityFrameworkCore;
using InventoryManagement.Repositories.Mock;
using InventoryManagement.DAL.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
//builder.Services.AddSingleton<ProductMockRepository>();
builder.Services.AddSingleton<SupplierMockRepository>();
builder.Services.AddSingleton<CategoryMockRepository>();
builder.Services.AddSingleton<WarehouseMockRepository>();
builder.Services.AddSingleton<UserMockRepository>();
builder.Services.AddSingleton<OrderMockRepository>();

builder.Services.AddDbContext<InventoryManagementDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("InventoryManagementDbContext")));

builder.Services.AddScoped<ProductEfRepository>();
builder.Services.AddScoped<SupplierEfRepository>();
builder.Services.AddScoped<CategoryEfRepository>();
builder.Services.AddScoped<WarehouseEfRepository>();
builder.Services.AddScoped<UserEfRepository>();
builder.Services.AddScoped<OrderEfRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
