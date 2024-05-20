using IlQuadrifoglio.Models;
using IlQuadrifoglio.Services;
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
        public async Task<IActionResult> Index()
        {
            var products = await _apiService.GetProductsAsync();
            if (products == null || !products.Any())
            {
                return View(new List<Product>());
            }
            ////Sortera efter typ av pizzor?

            return View(products);
        }
        public IActionResult Create()
        {
            return View();
        }
        // Post:Products/create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product)
        {
            if (ModelState.IsValid)
            {
                await _apiService.CreateProductAsync(product);
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }
        ////// GET: Products/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _apiService.GetProductByIdAsync(id);
            return View(product);
        }
        ////// Uppdate a product
        [HttpPost]
        public async Task<IActionResult> Edit(int id, Product product)
        {
            if (ModelState.IsValid)
            {
                await _apiService.UpdateProductAsync(id, product);
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }
        // details of product
        [HttpGet]
        public async Task<ActionResult> Details(int id)
        {
            var product = await _apiService.GetProductByIdAsync(id);
            return View(product);
        }

        [HttpPost, ActionName("DeleteConfirmed")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _apiService.DeleteProductAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}

