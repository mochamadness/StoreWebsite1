namespace PharmaCosmetics.Web.Models.Entities;

public class ProductActiveIngredient
{
    public int ProductId { get; set; }
    public Product Product { get; set; } = default!;
    public int ActiveIngredientId { get; set; }
    public ActiveIngredient ActiveIngredient { get; set; } = default!;
    public string? ConcentrationInfo { get; set; }
}