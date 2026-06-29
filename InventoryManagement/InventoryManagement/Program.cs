using InventoryManagement.DAL;
using InventoryManagement.DAL.Repositories;
using InventoryManagement.Domain.Models;
using InventoryManagement.Logging;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using InventoryManagement.Data;
using InventoryManagement.Services;
using System.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.AddFileLogger(builder.Environment.ContentRootPath, options =>
{
    options.Path = builder.Configuration["FileLogging:Path"] ?? options.Path;

    if (Enum.TryParse<LogLevel>(
        builder.Configuration["FileLogging:MinimumLevel"],
        ignoreCase: true,
        out var minimumLevel))
    {
        options.MinimumLevel = minimumLevel;
    }

    if (long.TryParse(
        builder.Configuration["FileLogging:MaxFileSizeBytes"],
        out var maxFileSizeBytes))
    {
        options.MaxFileSizeBytes = maxFileSizeBytes;
    }
});

// MVC + API controllers
builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient<IProductAiAdvisor, ProductAiAdvisor>();

var inventoryConnectionString = builder.Configuration.GetConnectionString("InventoryManagementDbContext");
var isTestingEnvironment = builder.Environment.IsEnvironment("Testing");

if (string.IsNullOrWhiteSpace(inventoryConnectionString) && !isTestingEnvironment)
{
    throw new InvalidOperationException(
        "Connection string 'InventoryManagementDbContext' is not configured. " +
        "Set ConnectionStrings:InventoryManagementDbContext or the " +
        "ConnectionStrings__InventoryManagementDbContext environment variable.");
}

builder.Services.AddDbContext<InventoryManagementDbContext>(options =>
{
    if (isTestingEnvironment && string.IsNullOrWhiteSpace(inventoryConnectionString))
    {
        options.UseInMemoryDatabase("InventoryManagementTestingBootstrap");
    }
    else
    {
        options.UseSqlServer(inventoryConnectionString);
    }
});

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

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";
});

var googleClientId = builder.Configuration["Authentication:Google:ClientId"];
var googleClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];

if (!builder.Environment.IsEnvironment("Testing") &&
    !string.IsNullOrWhiteSpace(googleClientId) &&
    !string.IsNullOrWhiteSpace(googleClientSecret))
{
    builder.Services
        .AddAuthentication()
        .AddGoogle(options =>
        {
            options.ClientId = googleClientId;
            options.ClientSecret = googleClientSecret;
            options.SaveTokens = true;
        });
}

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

using (var scope = app.Services.CreateScope())
{
    if (app.Configuration.GetValue<bool>("Database:ApplyMigrations"))
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<InventoryManagementDbContext>();
        await dbContext.Database.MigrateAsync();
    }

    await IdentitySeedData.SeedAsync(scope.ServiceProvider);
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

var requestLogger = app.Services
    .GetRequiredService<ILoggerFactory>()
    .CreateLogger("InventoryManagement.Requests");

app.Use(async (context, next) =>
{
    var stopwatch = Stopwatch.StartNew();

    try
    {
        await next();

        stopwatch.Stop();
        requestLogger.LogInformation(
            "HTTP {Method} {Path} responded {StatusCode} in {ElapsedMilliseconds} ms for {User}.",
            context.Request.Method,
            context.Request.Path,
            context.Response.StatusCode,
            stopwatch.ElapsedMilliseconds,
            context.User.Identity?.Name ?? "anonymous");
    }
    catch (Exception ex)
    {
        stopwatch.Stop();
        requestLogger.LogError(
            ex,
            "HTTP {Method} {Path} failed in {ElapsedMilliseconds} ms for {User}.",
            context.Request.Method,
            context.Request.Path,
            stopwatch.ElapsedMilliseconds,
            context.User.Identity?.Name ?? "anonymous");

        throw;
    }
});

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapGet("/health", () => Results.Ok(new
{
    status = "Healthy",
    application = "InventoryManagement"
}));

app.MapControllers();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();

public partial class Program
{
}
