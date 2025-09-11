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

    public DbSeeder(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task SeedAsync(IWebHostEnvironment env)
    {
        await _db.Database.MigrateAsync();

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
}