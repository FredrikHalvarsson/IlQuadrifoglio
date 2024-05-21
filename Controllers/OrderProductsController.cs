using IlQuadrifoglio.Models;
using IlQuadrifoglio.Services;
using Microsoft.AspNetCore.Mvc;

namespace IlQuadrifoglio.Controllers
{
    public class OrderProductsController : Controller
    {
        private readonly APIService _apiService;
        public OrderProductsController(APIService apiService)
        {
            _apiService = apiService;
        }
        public async Task<IActionResult> Index()
        {
            var orderProducts = await _apiService.GetOrderProductsAsync();
            if (orderProducts == null || !orderProducts.Any())
            {
                return View(new List<OrderProduct>());
            }

            return View(orderProducts);
        }
        // Get: OrderProducts/create
        public IActionResult Create()
        {
            return View();
        }
        // Post:OrderProducts/create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(OrderProduct orderProduct)
        {
            if (ModelState.IsValid)
            {
                await _apiService.CreateOrderProductAsync(orderProduct);
                return RedirectToAction(nameof(Index));
            }
            return View(orderProduct);
        }
        ////// GET: OrderProducts/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var orderProduct = await _apiService.GetOrderProductByIdAsync(id);
            return View(orderProduct);
        }
        ////// Uppdate an OrderProduct
        [HttpPost]
        public async Task<IActionResult> Edit(int id, OrderProduct orderProduct)
        {
            if (ModelState.IsValid)
            {
                await _apiService.UpdateOrderProductAsync(id, orderProduct);
                return RedirectToAction(nameof(Index));
            }
            return View(orderProduct);
        }
        // details of OrderProduct
        [HttpGet]
        public async Task<ActionResult> Details(int id)
        {
            var orderProduct = await _apiService.GetOrderProductByIdAsync(id);
            return View(orderProduct);
        }

        [HttpPost, ActionName("DeleteConfirmed")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _apiService.DeleteOrderProductAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
