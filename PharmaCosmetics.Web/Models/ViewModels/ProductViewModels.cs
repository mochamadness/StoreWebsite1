namespace PharmaCosmetics.Web.Models.ViewModels;

public class ProductListFilter
{
    public string? Query { get; set; }
    public string? Category { get; set; }
    public string? Brand { get; set; }
    public string? Tag { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 12;
}

public class ProductListItemVm
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? ShortDescription { get; set; }
    public string? PrimaryImageUrl { get; set; }
    public string? BrandName { get; set; }
    public string CategoryName { get; set; } = string.Empty;
}

public class PagedResult<T>
{
    public IEnumerable<T> Items { get; set; } = Enumerable.Empty<T>();
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalItems { get; set; }
    public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize);
}

public class ProductDetailsVm
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? ShortDescription { get; set; }
    public string? FullDescription { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public string? BrandName { get; set; }
    public List<string> Tags { get; set; } = new();
    public List<ProductImageVm> Images { get; set; } = new();
    public List<ActiveIngredientVm> ActiveIngredients { get; set; } = new();
}

public class ProductCreateVm
{
    public string Name { get; set; } = string.Empty;
    public string? ShortDescription { get; set; }
    public string? FullDescription { get; set; }
    public int CategoryId { get; set; }
    public int? BrandId { get; set; }
}

public class ProductEditVm
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? ShortDescription { get; set; }
    public string? FullDescription { get; set; }
    public int CategoryId { get; set; }
    public int? BrandId { get; set; }
}

public class ProductImageVm
{
    public string Url { get; set; } = string.Empty;
    public string? Caption { get; set; }
    public bool IsPrimary { get; set; }
}

public class ActiveIngredientVm
{
    public string Name { get; set; } = string.Empty;
    public string? ConcentrationInfo { get; set; }
}