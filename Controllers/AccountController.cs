using IlQuadrifoglio.Models;
using IlQuadrifoglio.Services;
using Microsoft.AspNetCore.Mvc;

namespace IlQuadrifoglio.Controllers
{
    public class AccountController : Controller
    {
        private readonly APIService _apiService;

        public AccountController(APIService apiService)
        {
            _apiService = apiService;
        }

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _apiService.LoginAsync(model.Username, model.Password);
                if (result)
                {
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Register() => View();

        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _apiService.RegisterAsync(model.Username, model.Password, model.Email);
                if (result)
                {
                    return RedirectToAction("Login");
                }
                ModelState.AddModelError(string.Empty, "Registration failed.");
            }
            return View(model);
        }
    }
}
