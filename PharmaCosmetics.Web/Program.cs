using Microsoft.EntityFrameworkCore;
using PharmaCosmetics.Web.Data;
using PharmaCosmetics.Web.Services;
using PharmaCosmetics.Web.Mappings;
using AutoMapper; // ensures Profile is in scope for AddProfile<T>()

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// AutoMapper registration
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<MappingProfile>();
});

builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IDateTimeProvider, SystemDateTimeProvider>();
builder.Services.AddScoped<IDbSeeder, DbSeeder>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var env = scope.ServiceProvider.GetRequiredService<IWebHostEnvironment>();
    var seeder = scope.ServiceProvider.GetRequiredService<IDbSeeder>();
    await seeder.SeedAsync(env);
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.MapControllerRoute(
    name: "product-details",
    pattern: "product/{slug}",
    defaults: new { controller = "Products", action = "Details" });

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Products}/{action=Index}/{id?}");

app.Run();