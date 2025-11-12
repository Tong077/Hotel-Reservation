using H_application.DTOs.GuestDto;
using H_application.DTOs.ReservationDto;
using H_application.DTOs.RoomDto;
using H_application.Service;
using H_Application.Service;
using H_Domain.DataContext;
using H_Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace H_Reservation.Controllers
{
    public class ReservationController : Controller
    {
        private readonly IReservationService _reservartion;
        private readonly IGustService _guest;
        private readonly IRoomService _room;
        private readonly EntityContext _context;
        public ReservationController(IReservationService reservartion, IGustService guest, IRoomService room, EntityContext context)
        {
            _context = context;
            _reservartion = reservartion;
            _guest = guest;
            _room = room;
        }
        public async Task<IActionResult> Index()
        {

            await LoadTotalRevenuse();

            var result = await _reservartion.GetAllReservationAsync("", default);
            return View("Index", result);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var guests = await _guest.GetAllAsync(default);

            // build FullName for dropdown
            var guestList = guests.Select(g => new
            {
                g.GuestId,
                FullName = g.FirstName + " " + g.LastName
            });

            ViewBag.Guests = new SelectList(guestList, "GuestId", "FullName");


            var model = new ReservationDtoCreate
            {
                Rooms = await _room.GetRoomStatusRoom(default)
            };


            var rooms = await _room.GetAvailableStatusRoom(default);

            ViewBag.Rooms = new SelectList(
                 rooms.Select(r => new
                 {
                     r.RoomId,
                     DisplayName = r.Currency switch
                     {
                         "USD" => $"{r.RoomNumber} - {r.RoomTypeName} - ${r.RoomPrice:F2}",
                         "KHR" => $"{r.RoomNumber} - {r.RoomTypeName} - {Math.Round(r.RoomPrice ?? 0)} Riel",
                         _ => $"{r.RoomNumber} - {r.RoomTypeName} - {r.RoomPrice}"
                     }
                 }),
                 "RoomId",
                 "DisplayName"
             );

            return View("Create", model);
        }

        [HttpPost]
        public async Task<IActionResult> Store(ReservationDtoCreate dtoCreate, CancellationToken cancellation)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData["toastr-type"] = "error";
                    TempData["toastr-message"] = "Can't Make a Reservation...!";
                    var guest = await _guest.GetAllAsync(default);
                    var guestLis = guest.Select(g => new
                    {
                        g.GuestId,
                        FullName = g.FirstName + " " + g.LastName
                    });

                    ViewBag.Guests = new SelectList(guestLis, "GuestId", "FullName");

                    var room = await _room.GetAvailableStatusRoom(default);

                    ViewBag.Rooms = new SelectList(
                         room.Select(r => new
                         {
                             r.RoomId,
                             DisplayName = r.Currency switch
                             {
                                 "USD" => $"{r.RoomNumber} - {r.RoomTypeName} - ${r.RoomPrice:F2}",
                                 "KHR" => $"{r.RoomNumber} - {r.RoomTypeName} - {Math.Round(r.RoomPrice ?? 0)} Riel",
                                 _ => $"{r.RoomNumber} - {r.RoomTypeName} - {r.RoomPrice}"
                             }
                         }),
                         "RoomId",
                         "DisplayName"
                     );

                    return View("Create", dtoCreate);
                }

                var result = await _reservartion.CreateReservationAsync(dtoCreate, cancellation);

                if (result)
                {
                    TempData["toastr-type"] = "success";
                    TempData["toastr-message"] = "The Reservation Success Fully...!";
                    return RedirectToAction("Index");
                }

                var guests = await _guest.GetAllAsync(default);
                var guestList = guests.Select(g => new
                {
                    g.GuestId,
                    FullName = g.FirstName + " " + g.LastName
                });

                ViewBag.Guests = new SelectList(guestList, "GuestId", "FullName");

                var rooms = await _room.GetAvailableStatusRoom(default);

                ViewBag.Rooms = new SelectList(
                     rooms.Select(r => new
                     {
                         r.RoomId,
                         DisplayName = r.Currency switch
                         {
                             "USD" => $"{r.RoomNumber} - {r.RoomTypeName} - ${r.RoomPrice:F2}",
                             "KHR" => $"{r.RoomNumber} - {r.RoomTypeName} - {Math.Round(r.RoomPrice ?? 0)} Riel",
                             _ => $"{r.RoomNumber} - {r.RoomTypeName} - {r.RoomPrice}"
                         }
                     }),
                     "RoomId",
                     "DisplayName"
                 );
                ModelState.AddModelError(string.Empty, "Can't make a Reservation.");
                return View("Create", dtoCreate);
            }
            catch (Exception ex)
            {
                // General error
                ModelState.AddModelError(string.Empty, $"Unexpected Error: {ex.Message}");
                Console.WriteLine(ex.ToString());
                return View("Create", dtoCreate);
            }

        }
        [HttpGet]
        public async Task<IActionResult> Edit(int Id)
        {
            var result = await _reservartion.GetReservationByIdAsync(Id, default);
            if (result == null) return NotFound();

            // Pass the full SelectedRoomIds list
            await PopulateDropdownsAsync(result.SelectedRoomIds);

            return View("Edit", result);
        }

        private async Task PopulateDropdownsAsync(List<int>? selectedRooms = null)
        {
            var guests = await _guest.GetAllAsync(default) ?? new List<GuestResponse>();
            ViewBag.Guests = new SelectList(
                guests.Select(g => new { g.GuestId, FullName = (g.FirstName ?? "") + " " + (g.LastName ?? "") }),
                "GuestId", "FullName"
            );

            var rooms = await _room.GetAvailableRoomsAsync(default) ?? new List<RoomResponse>();

            ViewBag.Rooms = rooms.Select(r => new SelectListItem
            {
                Value = r.RoomId.ToString(),
                Text = $"{r.RoomNumber} - {r.RoomTypeName}",
                Selected = selectedRooms?.Contains(r.RoomId) == true
            }).ToList();

            ViewBag.RoomsList = rooms;

            // ✅ Add full list of selected rooms for JS partial
            ViewBag.SelectedRoomIds = selectedRooms ?? new List<int>();
        }

        [HttpPost]
        public async Task<IActionResult> Update(ReservationDtoUpdate reservationDto, CancellationToken cancellation)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    await PopulateDropdownsAsync(reservationDto.RoomId);
                    return View("Edit", reservationDto);
                }

                var result = await _reservartion.UpdateReservationStatusAsync(reservationDto, cancellation);  // Fixed typo
                if (result)
                {
                    TempData["toastr-type"] = "success";
                    TempData["toastr-message"] = "The Reservation Has been Updated Successfully!";
                    return RedirectToAction("Index");
                }

                TempData["toastr-type"] = "error";
                TempData["toastr-message"] = "Cannot update reservation!";
                await PopulateDropdownsAsync(reservationDto.RoomId);
                return View("Edit", reservationDto);
            }
            catch (Exception ex)
            {
                var inner = ex.InnerException?.Message ?? ex.Message;
                ModelState.AddModelError("", "Unexpected error: " + inner);

                await PopulateDropdownsAsync(reservationDto.RoomId);
                return View("Edit", reservationDto);
            }
        }


        [HttpGet]
        public async Task<IActionResult> Delete(int Id)
        {
            var result = await _reservartion.GetReservationByIdAsync(Id, default);
            if (result == null) return NotFound();
            await PopulateDropdownsAsync(result.SelectedRoomIds);

            return View("Delete", result);
        }
        [HttpPost]
        public async Task<IActionResult> Destroy(ReservationDtoUpdate reservationDto, CancellationToken cancellation)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    await PopulateDropdownsAsync(reservationDto.RoomId);
                    return View("Delete", reservationDto);
                }
                var result = await _reservartion.DeleteReservationAsync(reservationDto, cancellation);
                if (result)
                {
                    return RedirectToAction("Index");
                }

                var guests = await _guest.GetAllAsync(default);

                // build FullName for dropdown
                var guestList = guests.Select(g => new
                {
                    g.GuestId,
                    FullName = g.FirstName + " " + g.LastName
                });

                ViewBag.Guests = new SelectList(guestList, "GuestId", "FullName");



                var rooms = await _room.GetAvailableRoomsAsync(default);

                ViewBag.Rooms = new SelectList(
                    rooms.Select(r => new
                    {
                        r.RoomId,
                        DisplayName = $"{r.RoomNumber} - {r.RoomTypeName}"
                    }),
                    "RoomId",
                    "DisplayName"
                );

                return View("Delete", result);
            }
            catch (Exception ex)
            {
                await PopulateDropdownsAsync(reservationDto.RoomId);
                ModelState.AddModelError("", "Unexpected error: " + ex.Message);
                return View("Delete", reservationDto);
            }

        }
        [HttpPost]
        public async Task<IActionResult> _Store(GuestDtoCreate dtoCreate, CancellationToken cancellation)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Where(x => x.Value!.Errors.Any())
                                       .ToDictionary(
                                           kvp => kvp.Key,
                                           kvp => kvp.Value!.Errors.Select(e => e.ErrorMessage).FirstOrDefault()
                                       );
                return Json(new { success = false, errors = errors });
            }

            var newGuestId = await _guest.guestCreateAsync(dtoCreate, cancellation);
            if (newGuestId != null)
            {
                return Json(new
                {
                    success = true,
                    newGuest = new
                    {
                        guestId = newGuestId,
                        fullName = dtoCreate.FirstName + " " + dtoCreate.LastName
                    }
                });
            }

            return Json(new { success = false, errors = new { Email = "Guest with same Email or Phone already exists." } });
        }


        [HttpGet]
        public async Task<IActionResult> GetGuests()
        {
            var guests = await _guest.GetAllAsync(default);
            var guestList = guests.Select(g => new SelectListItem
            {
                Value = g.GuestId.ToString(),
                Text = g.FirstName + " " + g.LastName
            }).ToList();

            return Json(guestList);
        }

        [HttpGet]
        public async Task<IActionResult> GetRevenueSummary(CancellationToken cancellation)
        {
            var currentYear = DateTime.Now.Year;
            var currentMonth = DateTime.Now.Month;

            // Get monthly revenue for USD and KHR
            var usdMonthly = await _reservartion.GetMonthlyRevenueByCurrencyAsync("USD", currentYear, cancellation);
            var khrMonthly = await _reservartion.GetMonthlyRevenueByCurrencyAsync("KHR", currentYear, cancellation);

            // Current month revenue
            ViewBag.USDMonth = usdMonthly
                .FirstOrDefault(x => x.Month == currentMonth)?.TotalRevenue ?? 0;

            ViewBag.KHRMonth = khrMonthly
                .FirstOrDefault(x => x.Month == currentMonth)?.TotalRevenue ?? 0;

            // Current year revenue (sum of all months)
            ViewBag.USDYear = usdMonthly.Sum(x => x.TotalRevenue);
            ViewBag.KHRYear = khrMonthly.Sum(x => x.TotalRevenue);

            // Growth percentage for current month compared to previous month
            ViewBag.USDGrowth = usdMonthly
                .FirstOrDefault(x => x.Month == currentMonth)?.GrowthPercentage ?? 0;

            ViewBag.KHRGrowth = khrMonthly
                .FirstOrDefault(x => x.Month == currentMonth)?.GrowthPercentage ?? 0;

            return View("GetRevenueSummary");
        }



        [HttpGet]
        public async Task<IActionResult> GetallInformation()
        {
            var reservationStats = await _reservartion.TotalReservation(default);
            ViewBag.TotalReservations = reservationStats.CurrentMonthTotal;
            ViewBag.GrowthPercentage = reservationStats.GrowthPercentage;

            var confirm = await _reservartion.ConfirmReservationAsync(default);
            ViewBag.Confirm = confirm.CurrentMonthTotal;

            var checkInTrend = await _reservartion.GetCheckInTrendAsync(default);
            ViewBag.Checkin = checkInTrend.CurrentMonthTotal;

            var pending = await _reservartion.PendingReservatoin(default);
            ViewBag.Pending = pending.CurrentMonthTotal;

            return View("GetallInformation");
        }

        [HttpGet]
        public async Task<IActionResult> GetRoomStatusRoom(CancellationToken cancellationToken)
        {
            var rooms = await _room.GetRoomStatusRoom(cancellationToken);
            return View("GetAvailableStatusRoom", rooms);
        }



        private async Task LoadTotalRevenuse()
        {
            var reservationStats = await _reservartion.TotalReservation(default);
            ViewBag.TotalReservations = reservationStats.CurrentMonthTotal;
            ViewBag.GrowthPercentage = reservationStats.GrowthPercentage;

            var confirm = await _reservartion.ConfirmReservationAsync(default);
            ViewBag.Confirm = confirm.CurrentMonthTotal;

            var checkInTrend = await _reservartion.GetCheckInTrendAsync(default);
            ViewBag.Checkin = checkInTrend.CurrentMonthTotal;

            var pending = await _reservartion.PendingReservatoin(default);
            ViewBag.Pending = pending.CurrentMonthTotal;

            var currentYear = DateTime.Now.Year;
            var currentMonth = DateTime.Now.Month;

            // Get monthly revenue for USD and KHR
            var usdMonthly = await _reservartion.GetMonthlyRevenueByCurrencyAsync("USD", currentYear, default);
            var khrMonthly = await _reservartion.GetMonthlyRevenueByCurrencyAsync("KHR", currentYear, default);

            // Current month revenue
            ViewBag.USDMonth = usdMonthly
                .FirstOrDefault(x => x.Month == currentMonth)?.TotalRevenue ?? 0;

            ViewBag.KHRMonth = khrMonthly
                .FirstOrDefault(x => x.Month == currentMonth)?.TotalRevenue ?? 0;

            // Current year revenue (sum of all months)
            ViewBag.USDYear = usdMonthly.Sum(x => x.TotalRevenue);
            ViewBag.KHRYear = khrMonthly.Sum(x => x.TotalRevenue);

            // Growth percentage for current month compared to previous month
            ViewBag.USDGrowth = usdMonthly
                .FirstOrDefault(x => x.Month == currentMonth)?.GrowthPercentage ?? 0;

            ViewBag.KHRGrowth = khrMonthly
                .FirstOrDefault(x => x.Month == currentMonth)?.GrowthPercentage ?? 0;
        }


        public async Task<IActionResult> _RoomCalendar(DateTime? startDate, CancellationToken cancellationToken)
        {
            var calendarData = await _reservartion.GetRoomCalendarAsync(startDate, cancellationToken);

            return PartialView("_RoomCalendar", calendarData);
        }

        [HttpGet]
        public async Task<IActionResult> GetCalendarData(DateTime startDate, DateTime endDate, CancellationToken cancellationToken)
        {
            var rooms = await _context.Rooms
                .Include(r => r.roomType)
                .ToListAsync(cancellationToken);

            var reservations = await _context.Reservations
                .Include(r => r.rooms)
                .Include(r => r.guest)
                .Where(r => r.CheckInDate != null && r.CheckInDate <= endDate &&
                            (r.CheckOutDate == null || r.CheckOutDate >= startDate))
                .ToListAsync(cancellationToken);

            var calendarData = new List<RoomCalendarDto>();

            foreach (var room in rooms)
            {
                for (var date = startDate.Date; date <= endDate.Date; date = date.AddDays(1))
                {

                    var reservation = reservations.FirstOrDefault(r =>
                        r.rooms != null &&
                        r.rooms.RoomNumber == room.RoomNumber && r.CheckInDate!.Value.Date <= date && (r.CheckOutDate == null || r.CheckOutDate.Value.Date >= date));


                    calendarData.Add(new RoomCalendarDto
                    {
                        ReservationId = reservation?.ReservationId ?? 0,
                        Date = date,
                        RoomNumber = room.RoomNumber,

                        RoomType = room.roomType?.Name ?? "Unknown",
                        GuestName = reservation?.guest != null
                        ? $"{reservation.guest.FirstName} {reservation.guest.LastName}"
                        : null,
                        Status = reservation != null ? reservation.Status : room.Status ?? " ",
                    });

                }
            }

            var today = DateTime.Today;
            var earliestCheckIn = reservations
                .Where(r => r.CheckInDate != null)
                .Min(r => r.CheckInDate!.Value.Date);


            var calendarStart = earliestCheckIn < today ? today : earliestCheckIn;

            return Json(new { calendarData, calendarStart });
        }

        [HttpPost]
        public async Task<IActionResult> UpdateStatus([FromBody] SwitchRoomRequest request, CancellationToken cancellationToken)
        {
            var reservation = await _context.Reservations.FindAsync(request.ReservationId);
            if (reservation == null) return NotFound("Reservation not found.");

            reservation.Status = request.NewStatus;
            await _context.SaveChangesAsync(cancellationToken);

            return Ok(new { success = true });
        }
        [HttpPost]
        public async Task<IActionResult> MoveReservation([FromBody] SwitchRoomRequest request, CancellationToken cancellationToken)
        {
            var reservation = await _context.Reservations
                .Include(r => r.rooms)
                .FirstOrDefaultAsync(r => r.rooms != null && r.rooms.RoomNumber == request.OldRoomNumber, cancellationToken);

            if (reservation == null)
                return NotFound("Reservation Not Found");

            //if (reservation.Status == "CheckedOut")
            //    return BadRequest("Cannot switch room for a checked-out reservation.");

            var newRoom = await _context.Rooms.FirstOrDefaultAsync(r => r.RoomNumber == request.NewRoomNumber);
            if (newRoom == null)
                return NotFound("Target room not found.");

            var newReservation = new Reservation
            {
                GuestId = reservation.GuestId,
                CheckInDate = reservation.CheckInDate,
                CheckOutDate = reservation.CheckOutDate,
                Status = reservation.Status,
                RoomId = newRoom.RoomId
            };

            _context.Reservations.Add(newReservation);
            var room = await _context.Rooms.FindAsync(new object[] { reservation.RoomId }, cancellationToken);
            if (room != null)
            {
                room.Status = reservation.Status switch
                {
                    "Confirmed" => "Occupied",
                    "CheckedIn" => "Occupied",
                    "Pending" => "Reserved",
                    "CheckedOut" => "Cleaning",
                    "Cancelled" => "Available",
                    _ => room.Status
                };
                _context.Rooms.Update(room);
            }
            await _context.SaveChangesAsync(cancellationToken);

            return Ok(new { message = "Room switched successfully." });
        }
    }
}