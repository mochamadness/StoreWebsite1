using PharmaCosmetics.Web.Models.ViewModels;

namespace PharmaCosmetics.Web.Services;

public interface IProductService
{
    Task<PagedResult<ProductListItemVm>> GetPagedProductsAsync(ProductListFilter filter);
    Task<ProductDetailsVm?> GetBySlugAsync(string slug);
}