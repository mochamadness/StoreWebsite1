namespace PharmaCosmetics.Web.Models.Entities;

public class Brand : BaseEntity
{
    public required string Name { get; set; }
    public string Slug { get; set; } = string.Empty;
    public ICollection<Product> Products { get; set; } = new List<Product>();
}