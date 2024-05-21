using IlQuadrifoglio.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;

namespace IlQuadrifoglio.Controllers
{
    public class LocationController : Controller
    {
        private readonly LocationService _locationService;
        private readonly ICompositeViewEngine _viewEngine;

        public LocationController(LocationService locationService, ICompositeViewEngine viewEngine)
        {
            _locationService = locationService;
            _viewEngine = viewEngine;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
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
