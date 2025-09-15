using PharmaCosmetics.Web.Models.ViewModels;
using PharmaCosmetics.Web.Models.Entities;

namespace PharmaCosmetics.Web.Services;

public interface IProductService
{
    Task<PagedResult<ProductListItemVm>> GetPagedProductsAsync(ProductListFilter filter);
    Task<ProductDetailsVm?> GetBySlugAsync(string slug);
    Task<ProductEditVm?> GetForEditAsync(int id);
    Task<int> CreateAsync(ProductCreateVm model);
    Task<bool> UpdateAsync(ProductEditVm model);
    Task<bool> SoftDeleteAsync(int id);
    Task<IEnumerable<Category>> GetCategoriesAsync();
    Task<IEnumerable<Brand>> GetBrandsAsync();
}