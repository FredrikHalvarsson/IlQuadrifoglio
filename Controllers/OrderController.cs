using IlQuadrifoglio.Models;
using IlQuadrifoglio.Services;
using Microsoft.AspNetCore.Mvc;

namespace IlQuadrifoglio.Controllers
{
    public class OrderController : Controller
    {
        private readonly APIService _apiService;
        public OrderController(APIService apiService)
        {
            _apiService = apiService;
        }
        public async Task<IActionResult> Index()
        {
            var orders = await _apiService.GetOrderAsync();
            if (orders == null || !orders.Any())
            {
                return View(new List<Order>());
            }

            return View(orders);
        }

        // Get: Orders/create
        public IActionResult Create()
        {
            return View();
        }

        // Post:Orders/create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Order order)
        {
            if (ModelState.IsValid)
            {
                await _apiService.CreateOrderAsync(order);
                return RedirectToAction(nameof(Index));
            }
            return View(order);
        }
        ////// GET: Orders/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var order = await _apiService.GetOrderByIdAsync(id);
            return View(order);
        }
        ////// Uppdate an order
        [HttpPost]
        public async Task<IActionResult> Edit(int id, Order order)
        {
            if (ModelState.IsValid)
            {
                await _apiService.UpdateOrderAsync(id, order);
                return RedirectToAction(nameof(Index));
            }
            return View(order);
        }
        // details of order
        [HttpGet]
        public async Task<ActionResult> Details(int id)
        {
            var order = await _apiService.GetOrderByIdAsync(id);
            return View(order);
        }

        [HttpPost, ActionName("DeleteConfirmed")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _apiService.DeleteOrderAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
