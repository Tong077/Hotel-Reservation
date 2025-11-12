using H_application.DTOs.ReservationServicesDto;
using H_application.Service;
using H_Domain.DataContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace H_Reservation.Controllers
{
    public class ReservationServicesController : Controller
    {
        private readonly IReservationServicesService _reservationService;
        private readonly IServicesService _service;
        private readonly IReservationService _reservation;
        private readonly EntityContext _Context;
        public ReservationServicesController(IReservationServicesService reservationService, IServicesService service, IReservationService reservation, EntityContext context)
        {
            _reservationService = reservationService;
            _service = service;
            _reservation = reservation;
            _Context = context;
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
            var services = await _Context.Services
                .Where(s => s.IsActive == true)
                .OrderBy(s => s.ServiceName)
                .ToListAsync();

            var servicelist = services.Select(s => new
            {
                s.ServiceId,
                DisplayText = $"Service Name: {s.ServiceName} | Price: {s.Price}"
            }).ToList();

            ViewBag.Service = new SelectList(servicelist, "ServiceId", "DisplayText");


            var reservations = await _reservation.GetAllReservationAsync("", default);
            var reservationSelectList = reservations.Select(r =>
            {
                var room = r.Rooms?.FirstOrDefault();
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
