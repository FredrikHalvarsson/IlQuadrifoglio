using IlQuadrifoglio.Models;
using IlQuadrifoglio.Services;
using Microsoft.AspNetCore.Mvc;

namespace IlQuadrifoglio.Controllers
{
    public class OrderController : Controller
    {
        private readonly APIService _apiService;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public OrderController(APIService apiService, IHttpContextAccessor httpContextAccessor)
        {
            _apiService = apiService;
            _httpContextAccessor = httpContextAccessor;
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

        // Get: Orders/Create
        public IActionResult Create()
        {
            var orderProducts = new List<OrderProduct>();

            // Skapa en ny Order-modell med de aktuella OrderProducts om de finns i sessionen
            var order = new Order
            {
                OrderProducts = orderProducts ?? new List<OrderProduct>()
            };

            return View(order);
        }

        // Post: Orders/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Order order)
        {
            if (ModelState.IsValid)
            {
                // Skicka endast Order till API:et, eftersom OrderProducts redan är bundna till ordern
                await _apiService.CreateOrderAsync(order);

                // Rensa OrderProducts från sessionen
                _httpContextAccessor.HttpContext.Session.Remove("OrderProducts");

                return RedirectToAction(nameof(Index));
            }
            return View(order);
        }

        // GET: Orders/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var order = await _apiService.GetOrderByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }

        // POST: Orders/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Order order)
        {
            if (id != order.OrderId)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                await _apiService.UpdateOrderAsync(id, order);
                return RedirectToAction(nameof(Index));
            }
            return View(order);
        }

        // GET: Orders/Details/5
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var order = await _apiService.GetOrderByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _apiService.DeleteOrderAsync(id);
            return RedirectToAction(nameof(Index));
        }


        // POST: Orders/DeleteOrderProduct/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteOrderProduct(int id)
        {
            var orderProduct = await _apiService.GetOrderProductByIdAsync(id);
            if (orderProduct != null)
            {
                await _apiService.DeleteOrderProductAsync(id);
            }
            return RedirectToAction(nameof(Index));
        }

    }
}
