using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using PharmaCosmetics.Web.Data;
using PharmaCosmetics.Web.Models.ViewModels;
using PharmaCosmetics.Web.Models.Entities;

namespace PharmaCosmetics.Web.Services;

public class ProductService : IProductService
{
    private readonly ApplicationDbContext _db;
    private readonly IMapper _mapper;

    public ProductService(ApplicationDbContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    public async Task<PagedResult<ProductListItemVm>> GetPagedProductsAsync(ProductListFilter filter)
    {
        var query = _db.Products
            .AsNoTracking()
            .Include(p => p.Images)
            .Include(p => p.Category)
            .Include(p => p.Brand)
            .Include(p => p.ProductTags).ThenInclude(pt => pt.Tag)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(filter.Query))
        {
            string q = filter.Query.Trim().ToLower();
            query = query.Where(p => p.Name.ToLower().Contains(q) || (p.ShortDescription != null && p.ShortDescription.ToLower().Contains(q)));
        }

        if (!string.IsNullOrWhiteSpace(filter.Category))
            query = query.Where(p => p.Category.Slug == filter.Category);

        if (!string.IsNullOrWhiteSpace(filter.Brand))
            query = query.Where(p => p.Brand != null && p.Brand.Slug == filter.Brand);

        if (!string.IsNullOrWhiteSpace(filter.Tag))
            query = query.Where(p => p.ProductTags.Any(pt => pt.Tag.Slug == filter.Tag));

        int total = await query.CountAsync();
        int skip = (filter.Page - 1) * filter.PageSize;

        var items = await query
            .OrderBy(p => p.Name)
            .Skip(skip)
            .Take(filter.PageSize)
            .ProjectTo<ProductListItemVm>(_mapper.ConfigurationProvider)
            .ToListAsync();

        return new PagedResult<ProductListItemVm>
        {
            Items = items,
            Page = filter.Page,
            PageSize = filter.PageSize,
            TotalItems = total
        };
    }

    public async Task<ProductDetailsVm?> GetBySlugAsync(string slug)
    {
        var entity = await _db.Products
            .AsNoTracking()
            .Include(p => p.Images)
            .Include(p => p.Category)
            .Include(p => p.Brand)
            .Include(p => p.ProductTags).ThenInclude(pt => pt.Tag)
            .Include(p => p.ProductActiveIngredients).ThenInclude(pi => pi.ActiveIngredient)
            .FirstOrDefaultAsync(p => p.Slug == slug);

        return entity == null ? null : _mapper.Map<ProductDetailsVm>(entity);
    }

    public async Task<ProductEditVm?> GetForEditAsync(int id)
    {
        var entity = await _db.Products
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id);

        return entity == null ? null : _mapper.Map<ProductEditVm>(entity);
    }

    public async Task<int> CreateAsync(ProductCreateVm model)
    {
        var entity = _mapper.Map<Product>(model);
        entity.Slug = SlugHelper.Generate(model.Name);

        _db.Products.Add(entity);
        await _db.SaveChangesAsync();
        return entity.Id;
    }

    public async Task<bool> UpdateAsync(ProductEditVm model)
    {
        var entity = await _db.Products.FindAsync(model.Id);
        if (entity == null) return false;

        _mapper.Map(model, entity);
        entity.Slug = SlugHelper.Generate(model.Name);

        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> SoftDeleteAsync(int id)
    {
        var entity = await _db.Products.FindAsync(id);
        if (entity == null) return false;

        entity.IsDeleted = true;
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<Category>> GetCategoriesAsync()
    {
        return await _db.Categories
            .AsNoTracking()
            .OrderBy(c => c.Name)
            .ToListAsync();
    }

    public async Task<IEnumerable<Brand>> GetBrandsAsync()
    {
        return await _db.Brands
            .AsNoTracking()
            .OrderBy(b => b.Name)
            .ToListAsync();
    }
}