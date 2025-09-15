using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PharmaCosmetics.Web.Models.Entities;
using PharmaCosmetics.Web.Services;

namespace PharmaCosmetics.Web.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    private readonly IDateTimeProvider _clock;
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IDateTimeProvider clock) : base(options)
    {
        _clock = clock;
    }

    public DbSet<Product> Products => Set<Product>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Brand> Brands => Set<Brand>();
    public DbSet<Tag> Tags => Set<Tag>();
    public DbSet<ProductTag> ProductTags => Set<ProductTag>();
    public DbSet<ActiveIngredient> ActiveIngredients => Set<ActiveIngredient>();
    public DbSet<ProductActiveIngredient> ProductActiveIngredients => Set<ProductActiveIngredient>();
    public DbSet<ProductImage> ProductImages => Set<ProductImage>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Global query filter for soft delete
        modelBuilder.Entity<Product>().HasQueryFilter(p => !p.IsDeleted);
        modelBuilder.Entity<Category>().HasQueryFilter(c => !c.IsDeleted);
        modelBuilder.Entity<Brand>().HasQueryFilter(b => !b.IsDeleted);
        modelBuilder.Entity<Tag>().HasQueryFilter(t => !t.IsDeleted);
        modelBuilder.Entity<ActiveIngredient>().HasQueryFilter(ai => !ai.IsDeleted);

        // ProductTag composite key
        modelBuilder.Entity<ProductTag>()
            .HasKey(pt => new { pt.ProductId, pt.TagId });

        modelBuilder.Entity<ProductTag>()
            .HasOne(pt => pt.Product)
            .WithMany(p => p.ProductTags)
            .HasForeignKey(pt => pt.ProductId);

        modelBuilder.Entity<ProductTag>()
            .HasOne(pt => pt.Tag)
            .WithMany(t => t.ProductTags)
            .HasForeignKey(pt => pt.TagId);

        // ProductActiveIngredient composite key
        modelBuilder.Entity<ProductActiveIngredient>()
            .HasKey(pi => new { pi.ProductId, pi.ActiveIngredientId });

        modelBuilder.Entity<ProductActiveIngredient>()
            .HasOne(pi => pi.Product)
            .WithMany(p => p.ProductActiveIngredients)
            .HasForeignKey(pi => pi.ProductId);

        modelBuilder.Entity<ProductActiveIngredient>()
            .HasOne(pi => pi.ActiveIngredient)
            .WithMany(ai => ai.ProductLinks)
            .HasForeignKey(pi => pi.ActiveIngredientId);

        // Index examples
        modelBuilder.Entity<Product>()
            .HasIndex(p => p.Slug)
            .IsUnique();

        modelBuilder.Entity<Category>()
            .HasIndex(c => c.Slug)
            .IsUnique();

        modelBuilder.Entity<Brand>()
            .HasIndex(b => b.Slug)
            .IsUnique();

        modelBuilder.Entity<Tag>()
            .HasIndex(t => t.Slug)
            .IsUnique();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var now = _clock.UtcNow;
        
        foreach (var entry in ChangeTracker.Entries<Models.Entities.BaseEntity>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedUtc = now;
                entry.Entity.UpdatedUtc = now;
            }
            else if (entry.State == EntityState.Modified)
            {
                entry.Entity.UpdatedUtc = now;
            }
        }

        foreach (var entry in ChangeTracker.Entries<ApplicationUser>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedUtc = now;
                entry.Entity.UpdatedUtc = now;
            }
            else if (entry.State == EntityState.Modified)
            {
                entry.Entity.UpdatedUtc = now;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}