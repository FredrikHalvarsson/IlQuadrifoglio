using IlQuadrifoglio.Services;
using Microsoft.AspNetCore.Mvc;
using IlQuadrifoglio.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace IlQuadrifoglio.Controllers
{
    public class ApplicationUsersController : Controller
    {
        private readonly UserService _userService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ApplicationUsersController(UserService userService, IHttpContextAccessor httpContextAccessor)
        {
            _userService = userService;
            _httpContextAccessor = httpContextAccessor;
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var users = await _userService.GetUsersAsync();

            return View(users);
        }

        // GET: Orders/Edit/5
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (id != userId)
            {
                return Unauthorized();
            }

            var user = await _userService.GetApplicationUserByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: Orders/Edit/5
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, ApplicationUser user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                await _userService.UpdateApplicationUserAsync(id, user);
                return RedirectToAction(nameof(Details), new { id = id });
            }
            return View(user);
        }

        // GET: Orders/Details/5
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Details()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var user = await _userService.GetApplicationUserByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (id != userId)
            {
                return Unauthorized();
            }

            await _userService.DeleteApplicationUserAsync(id);
            return RedirectToAction(nameof(Index));
        }


        // POST: Orders/DeleteApplicationUser/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteApplicationUserProduct(string id)
        {
            var user = await _userService.GetApplicationUserByIdAsync(id);
            if (user != null)
            {
                await _userService.DeleteApplicationUserAsync(id);
            }
            return RedirectToAction(nameof(Index));
        }

    }
}