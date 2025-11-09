using Microsoft.AspNetCore.Mvc;

namespace H_Reservation.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
