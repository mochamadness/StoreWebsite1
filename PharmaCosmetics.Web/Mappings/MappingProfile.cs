using AutoMapper;
using PharmaCosmetics.Web.Models.Entities;
using PharmaCosmetics.Web.Models.ViewModels;
using System.Linq;

namespace PharmaCosmetics.Web.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Product, ProductListItemVm>()
            .ForMember(d => d.PrimaryImageUrl, opt => opt.MapFrom(s =>
                // Avoid null-propagation (?.) inside expression trees
                s.Images
                    .Where(i => i.IsPrimary)
                    .OrderBy(i => i.SortOrder)
                    .Select(i => i.Url)
                    .FirstOrDefault()
                ?? s.Images
                    .OrderBy(i => i.SortOrder)
                    .Select(i => i.Url)
                    .FirstOrDefault()))
            .ForMember(d => d.BrandName, opt => opt.MapFrom(s => s.Brand != null ? s.Brand.Name : null))
            .ForMember(d => d.CategoryName, opt => opt.MapFrom(s => s.Category.Name));

        CreateMap<ProductImage, ProductImageVm>();

        CreateMap<Product, ProductDetailsVm>()
            .ForMember(d => d.BrandName, opt => opt.MapFrom(s => s.Brand != null ? s.Brand.Name : null))
            .ForMember(d => d.CategoryName, opt => opt.MapFrom(s => s.Category.Name))
            .ForMember(d => d.Tags, opt => opt.MapFrom(s => s.ProductTags.Select(pt => pt.Tag.Name)))
            .ForMember(d => d.Images, opt => opt.MapFrom(s => s.Images.OrderBy(i => i.SortOrder)))
            .ForMember(d => d.ActiveIngredients, opt => opt.MapFrom(s =>
                s.ProductActiveIngredients.Select(pi => new ActiveIngredientVm
                {
                    Name = pi.ActiveIngredient.Name,
                    ConcentrationInfo = pi.ConcentrationInfo
                })));

        // CRUD mappings
        CreateMap<ProductCreateVm, Product>();
        CreateMap<Product, ProductEditVm>();
        CreateMap<ProductEditVm, Product>();
    }
}