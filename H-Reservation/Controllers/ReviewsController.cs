using Microsoft.AspNetCore.Mvc;

namespace H_Reservation.Controllers
{
    public class ReviewsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
