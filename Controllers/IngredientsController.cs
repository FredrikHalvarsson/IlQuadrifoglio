using IlQuadrifoglio.Models;
using IlQuadrifoglio.Services;
using Microsoft.AspNetCore.Mvc;

namespace IlQuadrifoglio.Controllers
{
    public class IngredientsController : Controller
    {
        private readonly APIService _apiService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public IngredientsController(APIService apiService, IHttpContextAccessor httpContextAccessor)
        {
            _apiService = apiService;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<IActionResult> Index()
        {
            var ingredients = await _apiService.GetIngredientsAsync();
            if (ingredients == null || !ingredients.Any())
            {
                return View(new List<Ingredient>());
            }

            return View(ingredients);
        }
        // Get: Ingredients/create
        public IActionResult Create()
        {
            return View();
        }
        // Post:Ingredients/create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Ingredient ingredient)
        {
            if (ModelState.IsValid)
            {
                await _apiService.CreateIngredientAsync(ingredient);
                return RedirectToAction(nameof(Index));
            }
            return View(ingredient);
        }
        ////// GET: Ingredients/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var ingredient = await _apiService.GetIngredientByIdAsync(id);
            return View(ingredient);
        }
        ////// Uppdate an ingredient
        [HttpPost]
        public async Task<IActionResult> Edit(int id, Ingredient ingredient)
        {
            if (ModelState.IsValid)
            {
                await _apiService.UpdateIngredientAsync(id, ingredient);
                return RedirectToAction(nameof(Index));
            }
            return View(ingredient);
        }
        // details of Ingredient
        [HttpGet]
        public async Task<ActionResult> Details(int id)
        {
            var ingredient = await _apiService.GetIngredientByIdAsync(id);
            return View(ingredient);
        }

        [HttpPost, ActionName("DeleteConfirmed")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _apiService.DeleteIngredientAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}

