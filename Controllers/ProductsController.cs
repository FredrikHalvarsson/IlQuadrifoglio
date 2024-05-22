using IlQuadrifoglio.Models;
using IlQuadrifoglio.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IlQuadrifoglio.Controllers
{
    public class ProductsController : Controller
    {
        private readonly APIService _apiService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ProductsController(APIService apiService, IHttpContextAccessor httpContextAccessor)
        {
            _apiService = apiService;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<IActionResult> CustomerMenu()
        {
            var products = await _apiService.GetProductsAsync();
            if (products == null || !products.Any())
            {
                return View(new List<Product>());
            }
            ////Sortera efter typ av pizzor?

            return View(products);
        }
        //public async Task<IActionResult> AdminMenu()
        //{
        //    var products = await _apiService.GetProductsAsync();
        //    if (products == null || !products.Any())
        //    {
        //        return View(new List<Product>());
        //    }
        //    ////Sortera efter typ av pizzor?

        //    return View(products);
        //}
        [Authorize(Roles = "admin")]
        public IActionResult Create()
        {
            return View();
        }
        // Post:Products/create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Create(Product product)
        {
            if (ModelState.IsValid)
            {
                await _apiService.CreateProductAsync(product);
                return RedirectToAction(nameof(CustomerMenu));
            }
            return View(product);
        }
        ////// GET: Products/Edit/5
        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _apiService.GetProductByIdAsync(id);
            return View(product);
        }
        ////// Uppdate a product
        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int id, Product product)
        {
            if (ModelState.IsValid)
            {
                await _apiService.UpdateProductAsync(id, product);
                return RedirectToAction(nameof(CustomerMenu));
            }
            return View(product);
        }
        // details of product
        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Details(int id)
        {
            var product = await _apiService.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _apiService.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _apiService.DeleteProductAsync(id);
            return RedirectToAction(nameof(CustomerMenu));
        }

    }
}

