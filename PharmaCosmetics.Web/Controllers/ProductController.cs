using Microsoft.AspNetCore.Mvc;
using PharmaCosmetics.Web.Models.ViewModels;
using PharmaCosmetics.Web.Services;

namespace PharmaCosmetics.Web.Controllers;

public class ProductsController : Controller
{
    private readonly IProductService _productService;
    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    public async Task<IActionResult> Index([FromQuery] ProductListFilter filter)
    {
        var model = await _productService.GetPagedProductsAsync(filter);
        ViewBag.Filter = filter;
        return View(model);
    }

    public async Task<IActionResult> Details(string slug)
    {
        var product = await _productService.GetBySlugAsync(slug);
        if (product == null)
            return NotFound();
        return View(product);
    }
}