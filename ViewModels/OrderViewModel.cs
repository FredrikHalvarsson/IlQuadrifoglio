using IlQuadrifoglio.Models;

namespace IlQuadrifoglio.ViewModels
{
    public class OrderViewModel
    {
        public string UserName { get; set; }
        public List<Order> Orders { get; set; }
    }
}
