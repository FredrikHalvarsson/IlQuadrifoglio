using IlQuadrifoglio.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace IlQuadrifoglio.ViewModels
{
    public class OrderViewModel
    {
        public string UserName { get; set; }
        public IEnumerable<SelectListItem> Statuses { get; set; }
        public Order Order { get; set; }
    }
}
