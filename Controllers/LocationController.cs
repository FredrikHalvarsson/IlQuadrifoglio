using IlQuadrifoglio.Models;
using IlQuadrifoglio.Services;
using IlQuadrifoglio.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using System.Security.Claims;

namespace IlQuadrifoglio.Controllers
{
    public class LocationController : Controller
    {
        private readonly LocationService _locationService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public LocationController(LocationService locationService, IHttpContextAccessor httpContextAccessor)
        {
            _locationService = locationService;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userName = User.FindFirstValue(ClaimTypes.Name);
            var model = new IndexViewModel
            {
                UserName = userName
            };
            return View(model);
        }

        [HttpGet("geocode")]
        public async Task<IActionResult> Geocode(string address)
        {
            var locationData = await _locationService.GetGeolocationAsync(address);
            return Ok(locationData);
        }

        [HttpGet("route")]
        public async Task<IActionResult> Route(string origin, string destination)
        {
            var routeData = await _locationService.GetRouteAsync(origin, destination);
            return Ok(routeData);
        }

        [HttpPost]
        public async Task<IActionResult> Index(string address)
        {
            var locationData = await _locationService.GetGeolocationAsync(address);
            var model = new Dictionary<string, string>
            {
                { "clickedAddress", locationData }
            };
            return View(model);
        }
    }
}
