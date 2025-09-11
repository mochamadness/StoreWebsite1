namespace PharmaCosmetics.Web.Models.Entities;

public class Tag : BaseEntity
{
    public required string Name { get; set; }
    public string Slug { get; set; } = string.Empty;
    public ICollection<ProductTag> ProductTags { get; set; } = new List<ProductTag>();
}