using Microsoft.AspNetCore.Mvc;

namespace H_Reservation.Controllers
{
    public class BookingHistoryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
