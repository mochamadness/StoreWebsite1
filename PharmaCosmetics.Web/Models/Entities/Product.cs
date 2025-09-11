namespace PharmaCosmetics.Web.Models.Entities;

public class Product : BaseEntity
{
    public required string Name { get; set; }
    public string Slug { get; set; } = string.Empty;
    public string? ShortDescription { get; set; }
    public string? FullDescription { get; set; }
    public int CategoryId { get; set; }
    public Category Category { get; set; } = default!;
    public int? BrandId { get; set; }
    public Brand? Brand { get; set; }
    public ICollection<ProductImage> Images { get; set; } = new List<ProductImage>();
    public ICollection<ProductTag> ProductTags { get; set; } = new List<ProductTag>();
    public ICollection<ProductActiveIngredient> ProductActiveIngredients { get; set; } = new List<ProductActiveIngredient>();
}