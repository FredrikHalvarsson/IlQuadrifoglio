using IlQuadrifoglio.Models;
using IlQuadrifoglio.Services;
using Microsoft.AspNetCore.Mvc;

namespace IlQuadrifoglio.Controllers
{
    public class OrderProductsController : Controller
    {
        private readonly APIService _apiService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public OrderProductsController(APIService apiService, IHttpContextAccessor httpContextAccessor)
        {
            _apiService = apiService;
            _httpContextAccessor = httpContextAccessor;
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
        public async Task<IActionResult> Create(int productId, int quantity)
        {
            // Log the parameters to check if they are received correctly
            System.Diagnostics.Debug.WriteLine($"Received productId: {productId}, quantity: {quantity}");

            if (ModelState.IsValid)
            {
                var userName = _httpContextAccessor.HttpContext?.User?.Identity?.Name;
                System.Diagnostics.Debug.WriteLine($"Received userName: {userName}");

                var latestOrder = await _apiService.GetLatestOrderAsync(userName);

                var orderProduct = new OrderProduct
                {
                    FkOrderId = latestOrder.OrderId,
                    FkProductId = productId,
                    Quantity = quantity
                };

                var createOrderProductSuccess = await _apiService.CreateOrderProductAsync(orderProduct);


                if (!createOrderProductSuccess)
                {
                    ModelState.AddModelError("", "Failed to add the product to the order. Please try again.");
                    TempData["SuccessMessage"] = "Kunde inte lägga till i varukorgen";
                    return RedirectToAction("CustomerMenu", "Products");
                }

                TempData["SuccessMessage"] = "Varan är tillagd i varukorgen!";
                return RedirectToAction("CustomerMenu", "Products"); //om det fungerar
            }
            return RedirectToAction("Index", "Home"); //fungerar inte
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
                return RedirectToAction("Index", "Order");
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
            return RedirectToAction("Index", "Order");
        }
    }
}
