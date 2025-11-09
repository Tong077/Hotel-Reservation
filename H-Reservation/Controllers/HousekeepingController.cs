using H_application.DTOs.HousekeepingDto;
using H_application.Service;
using H_Domain.DataContext;
using H_Domain.Models;
using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace H_Reservation.Controllers
{
    public class HousekeepingController : Controller
    {
        private readonly IHousekeepingService _housekeepingService;
        private readonly IEmployeesService _employeesService;
        private readonly IRoomService _roomService;
        private readonly EntityContext _context;
        public HousekeepingController(IHousekeepingService housekeepingService, IEmployeesService employeesService, IRoomService roomService, EntityContext context)
        {
            _housekeepingService = housekeepingService;
            _employeesService = employeesService;
            _roomService = roomService;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _housekeepingService.GetHouseKeepingAll();
            return View("Index", result);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await LoadItem();
            return View("Create");
        }
        [HttpPost]
        public async Task<IActionResult> Store(HousekeepingDtoCreate dto)
        {
            if (!ModelState.IsValid)
            {
                await LoadItem();
                return View("Create", dto);
            }
            var reslut = await _housekeepingService.CreateHouseKeeping(dto);
            if (reslut)
            {
                return RedirectToAction("Index");
            }
            return View("Create", dto);
        }
       
        [HttpGet]
        public async Task<IActionResult> Edit(int Id)
        {
            await LoadItem();

            var result = await _housekeepingService.GetHouseKeepingById(Id);
            if (result == null)
            {
                return NotFound();
            }

            return View("Edit", result);
        }

        [HttpPost]
        public async Task<IActionResult> Update(HousekeepingDtoUpdate dto)
        {

            if (!ModelState.IsValid)
            {
                await LoadItem();
                return View("Edit", dto);
            }
            var reslut = await _housekeepingService.UpdateHouseKeeping(dto);
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

            var result = await _housekeepingService.GetHouseKeepingById(Id);
            if (result == null)
                return NotFound();

            return View("Delete", result);
        }

        [HttpPost]
        public async Task<IActionResult> Destroy(HousekeepingDtoUpdate dto)
        {
            if (!ModelState.IsValid)
            {
                await LoadItem();
                return View("Delete", dto);
            }
            var reslut = await _housekeepingService.DeleteHouseKeeping(dto);
            if (reslut)
            {
                return RedirectToAction("Index");
            }
            return View("Delete", dto);
        }
        private async Task LoadItem()
        {
            var rooms = await _roomService.GetAvailableRoomsAsync(default);
            ViewBag.Room = new SelectList(
                     rooms.Select(r => new
                     {
                         r.RoomId,
                         DisplayName = $"{r.RoomNumber} - {r.RoomTypeName}"
                     }),
                     "RoomId",
                     "DisplayName"
                 );

            var employee = await _employeesService.GetEmployees();
            ViewBag.Employee = new SelectList(employee, "EmployeeId", "FullName");

        }
        [HttpPost]
        public async Task<IActionResult> BulkUpdateStatus(List<int> selectedRoomIds, string status, CancellationToken cancellationToken)
        {
            if (selectedRoomIds == null || !selectedRoomIds.Any() || string.IsNullOrEmpty(status))
            {
                TempData["Error"] = "Please select at least one room and a status.";
                return RedirectToAction(nameof(Index));
            }

            var rooms = await _context.Housekeeping
                .Where(r => selectedRoomIds.Contains(r.HousekeepingId))
                .ToListAsync(cancellationToken);

            if (!rooms.Any())
            {
                TempData["Error"] = "No matching rooms found.";
                return RedirectToAction(nameof(Index));
            }

            foreach (var room in rooms)
            {
                room.Status = status;
            }

            await _context.SaveChangesAsync(cancellationToken);

            TempData["Success"] = $"{rooms.Count} House(s) updated to '{status}' successfully.";
            return RedirectToAction(nameof(Index));
        }

    }
}
