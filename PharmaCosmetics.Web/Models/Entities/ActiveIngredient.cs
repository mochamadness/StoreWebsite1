namespace PharmaCosmetics.Web.Models.Entities;

public class ActiveIngredient : BaseEntity
{
    public required string Name { get; set; }
    public string? Description { get; set; }
    public ICollection<ProductActiveIngredient> ProductLinks { get; set; } = new List<ProductActiveIngredient>();
}