using H_application.DTOs.ServicesDto;
using H_application.Service;
using Microsoft.AspNetCore.Mvc;

namespace H_Reservation.Controllers
{
    public class ServicesController : Controller
    {
        private readonly IServicesService _servicesService;

        public ServicesController(IServicesService servicesService)
        {
            _servicesService = servicesService;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _servicesService.GetallService();
            return View("Index", result);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {

            return View("Create");
        }
        [HttpPost]
        public async Task<IActionResult> Store(ServicesDtoCreate dto)
        {
            if (!ModelState.IsValid)
            {

                return View("Create", dto);
            }
            var reslut = await _servicesService.CreateService(dto);
            if (reslut)
            {
                return RedirectToAction("Index");
            }
            return View("Create", dto);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int Id)
        {
            ;

            var result = await _servicesService.GetServiceById(Id, default);
            if (result == null)
            {
                return NotFound();
            }

            return View("Edit", result);
        }

        [HttpPost]
        public async Task<IActionResult> Update(ServicesDtoUpdate dto)
        {

            if (!ModelState.IsValid)
            {

                return View("Edit", dto);
            }
            var reslut = await _servicesService.UpdateService(dto);
            if (reslut)
            {
                return RedirectToAction("Index");
            }
            return View("Edit", dto);
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int Id)
        {


            var result = await _servicesService.GetServiceById(Id);
            if (result == null)
                return NotFound();

            return View("Delete", result);
        }

        [HttpPost]
        public async Task<IActionResult> Destroy(ServicesDtoUpdate dto)
        {
            if (!ModelState.IsValid)
            {

                return View("Delete", dto);
            }
            var reslut = await _servicesService.DeleteService(dto);
            if (reslut)
            {
                return RedirectToAction("Index");
            }
            return View("Delete", dto);
        }
    }
}
