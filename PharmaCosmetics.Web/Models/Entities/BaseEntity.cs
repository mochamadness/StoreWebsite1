namespace PharmaCosmetics.Web.Models.Entities;

public abstract class BaseEntity
{
    public int Id { get; set; }
    public DateTime CreatedUtc { get; set; }
    public DateTime UpdatedUtc { get; set; }
    public bool IsDeleted { get; set; } = false;
}