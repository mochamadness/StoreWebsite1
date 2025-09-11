namespace PharmaCosmetics.Web.Models.Entities;

public class ProductImage : BaseEntity
{
    public required string Url { get; set; }
    public string? Caption { get; set; }
    public bool IsPrimary { get; set; }
    public int SortOrder { get; set; } = 0;
    public int ProductId { get; set; }
    public Product Product { get; set; } = default!;
}