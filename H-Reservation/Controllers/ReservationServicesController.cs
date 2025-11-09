using H_application.DTOs.ReservationServicesDto;
using H_application.DTOs.ServicesDto;
using H_application.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace H_Reservation.Controllers
{
    public class ReservationServicesController : Controller
    {
        private readonly IReservationServicesService _reservationService;
        private readonly IServicesService _service;
        private readonly IReservationService _reservation;
        public ReservationServicesController(IReservationServicesService reservationService, IServicesService service, IReservationService reservation)
        {
            _reservationService = reservationService;
            _service = service;
            _reservation = reservation;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _reservationService.GetAllReservationService();
            return View("Index", result);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await LoadItem();
            return View("Create");
        }
        [HttpPost]
        public async Task<IActionResult> Store(ReservationServicesDtoCreate dto)
        {
            if (!ModelState.IsValid)
            {
                await LoadItem();

                return View("Create", dto);
            }
            var reslut = await _reservationService.CreateReservationService(dto);
            if (reslut)
            {
                return RedirectToAction("Index");
            }
            await LoadItem();
            return View("Create", dto);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int Id)
        {
            await LoadItem();
            var result = await _reservationService.GetReservationById(Id, default);
            if (result == null)
            {
                return NotFound();
            }

            return View("Edit", result);
        }

        [HttpPost]
        public async Task<IActionResult> Update(ReservationServicesDtoUpdate dto)
        {

            if (!ModelState.IsValid)
            {
                await LoadItem();

                return View("Edit", dto);
            }
            await LoadItem();
            var reslut = await _reservationService.UpdateReservationService(dto);
            if (reslut)
            {
                return RedirectToAction("Index");
            }
            return View("Edit", dto);
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int Id)
        {
            await LoadItem();
            var result = await _reservationService.GetReservationById(Id);
            if (result == null)
                return NotFound();

            return View("Delete", result);
        }

        [HttpPost]
        public async Task<IActionResult> Destroy(ReservationServicesDtoUpdate dto)
        {
            if (!ModelState.IsValid)
            {
                await LoadItem();

                return View("Delete", dto);
            }
            var reslut = await _reservationService.DeleteReservationService(dto);
            if (reslut)
            {
                return RedirectToAction("Index");
            }
            return View("Delete", dto);
        }
        private async Task LoadItem()
        {
            var service = await _service.GetallService();

            var servicelist = service.Select(s =>
            {
                var servicePrice = s.Price;
                var serviceName = s.ServiceName;
                return new
                {
                    s.ServiceId,
                    DisplayText = $"Service Name: {serviceName} | Price: {servicePrice}"
                };
            }).ToList();
            ViewBag.Service = new SelectList(servicelist, "ServiceId", "DisplayText");

            var reservations = await _reservation.GetAllReservationAsync("", default);
            var reservationSelectList = reservations.Select(r =>
            {
                var room = r.Rooms?.FirstOrDefault(); // take the first room
                var roomNumber = room?.RoomNumber ?? "N/A";
                var roomTypeName = room?.RoomTypeName ?? "N/A";

                var guestName = $"{r.FirstName} {r.LastName}";

                return new
                {
                    r.ReservationId,
                    DisplayText = $"Guest: {guestName} | Room: {roomNumber} ({roomTypeName}) | Total: ${r.TotalPrice}"
                };
            }).ToList();

            ViewBag.Reservation = new SelectList(reservationSelectList, "ReservationId", "DisplayText");



        }
    }
}
