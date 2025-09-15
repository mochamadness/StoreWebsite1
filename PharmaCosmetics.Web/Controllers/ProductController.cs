using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create()
    {
        await PopulateDropdowns();
        return View(new ProductCreateVm());
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ProductCreateVm model)
    {
        if (!ModelState.IsValid)
        {
            await PopulateDropdowns();
            return View(model);
        }

        var id = await _productService.CreateAsync(model);
        TempData["SuccessMessage"] = "Product created successfully.";
        return RedirectToAction(nameof(Index));
    }

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Edit(int id)
    {
        var model = await _productService.GetForEditAsync(id);
        if (model == null)
            return NotFound();

        await PopulateDropdowns();
        return View(model);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(ProductEditVm model)
    {
        if (!ModelState.IsValid)
        {
            await PopulateDropdowns();
            return View(model);
        }

        var success = await _productService.UpdateAsync(model);
        if (!success)
            return NotFound();

        TempData["SuccessMessage"] = "Product updated successfully.";
        return RedirectToAction(nameof(Index));
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _productService.SoftDeleteAsync(id);
        if (!success)
            return NotFound();

        TempData["SuccessMessage"] = "Product deleted successfully.";
        return RedirectToAction(nameof(Index));
    }

    private async Task PopulateDropdowns()
    {
        var categories = await _productService.GetCategoriesAsync();
        var brands = await _productService.GetBrandsAsync();

        ViewBag.Categories = new SelectList(categories, "Id", "Name");
        ViewBag.Brands = new SelectList(brands, "Id", "Name");
    }
}