using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PharmaCosmetics.Web.Models.Entities;
using PharmaCosmetics.Web.Services;

namespace PharmaCosmetics.Web.Data;

public interface IDbSeeder
{
    Task SeedAsync(IWebHostEnvironment env);
}

public class DbSeeder : IDbSeeder
{
    private readonly ApplicationDbContext _db;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public DbSeeder(ApplicationDbContext db, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _db = db;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task SeedAsync(IWebHostEnvironment env)
    {
        await _db.Database.MigrateAsync();

        // Seed roles
        await SeedRolesAsync();

        // Seed admin user
        await SeedAdminUserAsync();

        // Seed sample data if no products exist
        if (await _db.Products.AnyAsync())
            return;

        var skincare = new Category { Name = "Skincare", Slug = SlugHelper.Generate("Skincare") };
        var sunscreenCat = new Category { Name = "Sunscreen", Slug = SlugHelper.Generate("Sunscreen"), ParentCategory = skincare };
        var brand = new Brand { Name = "DermaLife", Slug = SlugHelper.Generate("DermaLife") };
        var niacinamide = new ActiveIngredient { Name = "Niacinamide", Description = "Vitamin B3 derivative." };
        var zincOxide = new ActiveIngredient { Name = "Zinc Oxide", Description = "Physical UV filter." };
        var tagVegan = new Tag { Name = "Vegan", Slug = "vegan" };
        var tagSPF = new Tag { Name = "SPF50", Slug = "spf50" };

        var product = new Product
        {
            Name = "DermaLife Daily Mineral Sunscreen SPF 50",
            Slug = SlugHelper.Generate("DermaLife Daily Mineral Sunscreen SPF 50"),
            Category = sunscreenCat,
            Brand = brand,
            ShortDescription = "Broad-spectrum mineral sunscreen with niacinamide.",
            FullDescription = "A lightweight mineral sunscreen offering SPF 50 protection with soothing niacinamide.",
            Images =
            {
                new ProductImage { Url = "/images/sample/sunscreen1-main.jpg", IsPrimary = true, SortOrder = 0 },
                new ProductImage { Url = "/images/sample/sunscreen1-texture.jpg", IsPrimary = false, SortOrder = 1 }
            },
            ProductTags =
            {
                new ProductTag { Tag = tagVegan },
                new ProductTag { Tag = tagSPF }
            },
            ProductActiveIngredients =
            {
                new ProductActiveIngredient { ActiveIngredient = niacinamide, ConcentrationInfo = "5%" },
                new ProductActiveIngredient { ActiveIngredient = zincOxide, ConcentrationInfo = "18%" }
            }
        };

        _db.Products.Add(product);
        await _db.SaveChangesAsync();
    }

    private async Task SeedRolesAsync()
    {
        if (!await _roleManager.RoleExistsAsync("Admin"))
        {
            await _roleManager.CreateAsync(new IdentityRole("Admin"));
        }

        if (!await _roleManager.RoleExistsAsync("User"))
        {
            await _roleManager.CreateAsync(new IdentityRole("User"));
        }
    }

    private async Task SeedAdminUserAsync()
    {
        const string adminEmail = "admin@pharmacosmetics.com";
        
        if (await _userManager.FindByEmailAsync(adminEmail) == null)
        {
            var adminUser = new ApplicationUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                FirstName = "Admin",
                LastName = "User",
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(adminUser, "Admin123!");
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }
    }
}