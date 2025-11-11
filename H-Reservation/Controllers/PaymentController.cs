using H_application.DTOs.Payment;
using H_application.DTOs.PaymentDto;
using H_application.Service;
using H_Domain.DataContext;
using H_Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace H_Reservation.Controllers
{
    public class PaymentController : Controller
    {
        private readonly IPaymentService _service;
        private readonly IPaymentMethodService _paymethod;
        private readonly IReservationService reservation;
        private readonly EntityContext _context;
        private readonly IInvoicesServicecs _invoices;

        public PaymentController(IPaymentService service, IPaymentMethodService paymethod, IReservationService reservation, EntityContext entity, IInvoicesServicecs invoices)
        {
            _service = service;
            _paymethod = paymethod;
            this.reservation = reservation;
            this._context = entity;
            _invoices = invoices;
        }
        public async Task<IActionResult> Index()
        {

            await TotalPay();
            var payment = await _service.GetAllPaymentResponsesAsync(default);
            return View("Index", payment);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var paymentMethods = await _paymethod.GetAllPaymentMethods(default);
            ViewBag.PaymentMethods = new SelectList(paymentMethods, "PaymentMethodId", "Name");

            await LoadRoom();
            return View("Create");
        }

        [HttpPost]
        public async Task<IActionResult> Store(PaymentDtoCreate create)
        {
            // 1️⃣ Model validation
            if (!ModelState.IsValid)
            {
                // Collect all field-specific errors
                var errors = ModelState
                    .Where(ms => ms.Value.Errors.Count > 0)
                    .Select(ms => new
                    {
                        Field = ms.Key,
                        Errors = ms.Value.Errors.Select(e => e.ErrorMessage).ToList()
                    })
                    .ToList();

                // Build a detailed message
                var detailedMessage = string.Join(" | ", errors.Select(e => $"{e.Field}: {string.Join(", ", e.Errors)}"));

                // Log to console (optional)
                Console.WriteLine("ModelState Errors: " + detailedMessage);

                // Pass to TempData for view
                TempData["ModelStateErrors"] = detailedMessage;

                TempData["toastr-type"] = "error";
                TempData["toastr-message"] = "The Payment Could Not Be Created! See detailed errors below.";

                var paymentMethods = await _paymethod.GetAllPaymentMethods(default);
                ViewBag.PaymentMethods = new SelectList(paymentMethods, "PaymentMethodId", "Name");

                await LoadRoom();
                return View("Create", create);
            }



            var result = await _service.CreatePaymentAsync(create, default);
            if (result)
            {
                TempData["toastr-type"] = "success";
                TempData["toastr-message"] = "The Payment Has Been Created Successfully!";
                return RedirectToAction("Index");
            }

            TempData["toastr-type"] = "error";
            TempData["toastr-message"] = "The Payment Could Not Be Saved.";
            return View("Create", create);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int Id)
        {
            var result = await _service.GetPaymentByIdAsync(Id, default);
            if (result == null)
                return NotFound();

            if (result.PaymentStatus == "Completed")
            {
                TempData["toastr-type"] = "warning";
                TempData["toastr-message"] = "Completed payments cannot be modified.";
                return RedirectToAction("Index");
            }

            var paymentMethods = await _paymethod.GetAllPaymentMethods(default);
            ViewBag.PaymentMethods = new SelectList(paymentMethods, "PaymentMethodId", "Name", result.PaymentMethodId);

            // Load reservations
            var reservations = await _context.Reservations
                .Include(r => r.guest)
                .Include(r => r.rooms)
                    .ThenInclude(rm => rm!.roomType)
                .Include(r => r.ReservationServices)
                    .ThenInclude(rs => rs.Service)
                .Where(r => r.PaymentId == null || r.PaymentId == result.PaymentId)
                .ToListAsync();

            var reservationList = reservations.Select(r =>
            {
                var guestName = r.guest != null ? $"{r.guest.FirstName} {r.guest.LastName}" : "Unknown Guest";
                var roomNumber = r.rooms?.RoomNumber ?? "Unknown Room";
                var roomPrice = r.rooms?.roomType?.PricePerNight ?? 0;
                var servicesTotal = r.ReservationServices?.Sum(rs => rs.TotalPrice ?? 0) ?? 0;
                var total = roomPrice + servicesTotal;
                var currency = r.rooms?.roomType?.Currency ?? r.Currency ?? "USD";

                return new ReservationForPaymentDto
                {
                    ReservationId = r.ReservationId,
                    GuestFullName = guestName,
                    RoomNumber = roomNumber,
                    TotalPrice = total,
                    Currency = currency,
                    DisplayTotal = currency == "USD" ? $"${total:N2}" : $"៛{total:N0}"
                };
            }).ToList();

            ViewBag.Reservations = reservationList;
            ViewBag.SelectedReservationIds = result.ReservationId ?? new List<int>();

            // Calculate initial total
            result.Amount = reservationList
                .Where(r => result.ReservationId.Contains(r.ReservationId))
                .Sum(r => r.TotalPrice);

            return View("Edit", result);
        }


        //[HttpPost]
        //public async Task<IActionResult> Update(PaymentDtoUpdate update)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        await LoadViewBagsAsync(update.PaymentMethodId);
        //        return View("Edit", update);
        //    }

        //    var existingPayment = await _service.GetPaymentByIdAsync(update.PaymentId, default);
        //    if (existingPayment == null)
        //    {
        //        TempData["toastr-type"] = "error";
        //        TempData["toastr-message"] = "Payment not found.";
        //        return RedirectToAction("Index");
        //    }

        //    if (existingPayment.PaymentStatus == "Completed")
        //    {
        //        TempData["toastr-type"] = "warning";
        //        TempData["toastr-message"] = "Completed payments cannot be modified.";
        //        return RedirectToAction("Index");
        //    }

        //    var success = await _service.UpdatePaymentWithInvoiceAsync(update, default);

        //    if (success)
        //    {
        //        TempData["toastr-type"] = "success";
        //        TempData["toastr-message"] = "Payment updated successfully!";
        //        return RedirectToAction("Index");
        //    }

        //    TempData["toastr-type"] = "error";
        //    TempData["toastr-message"] = "Failed to update payment.";
        //    await LoadViewBagsAsync(update.PaymentMethodId);
        //    return View("Edit", update);
        //}
        [HttpPost]
        public async Task<IActionResult> Update(PaymentDtoUpdate update)
        {
            if (!ModelState.IsValid)
            {
                await LoadViewBagsAsync(update.PaymentMethodId);
                return View("Edit", update);
            }

            var existingPayment = await _service.GetPaymentByIdAsync(update.PaymentId, default);
            if (existingPayment == null)
            {
                TempData["toastr-type"] = "error";
                TempData["toastr-message"] = "Payment not found.";
                return RedirectToAction("Index");
            }

            if (existingPayment.PaymentStatus == "Completed")
            {
                TempData["toastr-type"] = "warning";
                TempData["toastr-message"] = "Completed payments cannot be modified.";
                return RedirectToAction("Index");
            }

            // Recalculate Amount from selected reservations
            var selectedReservations = await _context.Reservations
                .Include(r => r.rooms)
                    .ThenInclude(rm => rm!.roomType)
                .Include(r => r.ReservationServices)
                .Where(r => update.ReservationId.Contains(r.ReservationId))
                .ToListAsync();

            update.Amount = selectedReservations.Sum(r =>
                (r.rooms?.roomType?.PricePerNight ?? 0) +
                (r.ReservationServices?.Sum(rs => rs.TotalPrice ?? 0) ?? 0)
            );

            var success = await _service.UpdatePaymentWithInvoiceAsync(update, default);

            if (success)
            {
                TempData["toastr-type"] = "success";
                TempData["toastr-message"] = "Payment updated successfully!";
                return RedirectToAction("Index");
            }

            TempData["toastr-type"] = "error";
            TempData["toastr-message"] = "Failed to update payment.";
            await LoadViewBagsAsync(update.PaymentMethodId);
            return View("Edit", update);
        }



        //[HttpPost]
        //public async Task<IActionResult> Update(PaymentDtoUpdate update)
        //{
        //    try
        //    {
        //        if (!ModelState.IsValid)
        //        {
        //            await LoadViewBagsAsync(update.PaymentMethodId);
        //            return View("Edit", update);
        //        }


        //        var existingPayment = await _service.GetPaymentByIdAsync(update.PaymentId, default);
        //        if (existingPayment == null)
        //        {
        //            TempData["toastr-type"] = "error";
        //            TempData["toastr-message"] = "Payment not found.";
        //            return RedirectToAction("Index");
        //        }

        //        if (existingPayment.PaymentStatus == "Completed")
        //        {
        //            TempData["toastr-type"] = "warning";
        //            TempData["toastr-message"] = "Completed payments cannot be modified.";
        //            await _invoices.CreateInvoiceAsync(update.ReservationId ?? 0);
        //            return RedirectToAction("Index");
        //        }

        //        var result = await _service.UpdatePaymentWithInvoiceAsync(update, default);
        //        if (result)
        //        {
        //            TempData["toastr-type"] = "success";
        //            TempData["toastr-message"] = "The payment has been updated successfully!";
        //            return RedirectToAction("Index");
        //        }

        //        await LoadViewBagsAsync(update.PaymentMethodId);
        //        return View("Edit", update);
        //    }
        //    catch (Exception ex)
        //    {
        //        ModelState.AddModelError("", ex.Message);
        //        await LoadViewBagsAsync(update.PaymentMethodId);
        //        return View("Edit", update);
        //    }
        //}

        [HttpGet]
        public async Task<IActionResult> Delete(int Id)
        {
            var result = await _service.GetPaymentByIdAsync(Id, default);
            if (result == null) return NotFound();


            var paymentMethods = await _paymethod.GetAllPaymentMethods(default);
            ViewBag.PaymentMethods = new SelectList(paymentMethods, "PaymentMethodId", "Name", result.PaymentMethodId);


            // Get all reservations
            var reservations = _context.Reservations
                .Select(r => new
                {
                    r.ReservationId,
                    GuestFullName = r.guest != null
                        ? r.guest.FirstName + " " + r.guest.LastName
                        : "Unknown Guest",

                    RoomNumber = r.rooms != null
                        ? r.rooms.RoomNumber ?? "Unknown"
                        : "Unknown Room",

                    Currency = r.rooms != null && r.rooms.roomType != null
                        ? r.rooms.roomType.Currency
                        : r.Currency,

                    TotalPrice = r.rooms != null && r.rooms.roomType != null
                        ? r.rooms.roomType.PricePerNight
                        : r.TotalPrice ?? 0
                })
                .AsEnumerable()
                .Select(r => new
                {
                    r.ReservationId,
                    r.GuestFullName,
                    r.RoomNumber,
                    DisplayTotal = r.Currency == "USD"
                        ? $"${r.TotalPrice:N2}"
                        : $"៛{r.TotalPrice:N0}",
                    r.Currency,
                    r.TotalPrice
                })
                .ToList();


            var selectedReservationIds = result.ReservationId ?? new List<int>();


            ViewBag.Reservations = reservations;
            ViewBag.SelectedReservationIds = selectedReservationIds;

            return View(result);
        }

        [HttpPost]
        public async Task<IActionResult> Destroy(PaymentDtoUpdate delete)
        {
            if (!ModelState.IsValid)
            {
                return View("Delete", delete);
            }

            var result = await _service.DeletePaymentAsync(delete, default);

            if (result)
            {
                TempData["toastr-type"] = "success";
                TempData["toastr-message"] = "The Payment Has Been Deleted Successfully!";
                return RedirectToAction("Index");
            }

            var reservations = _context.Reservations
                 .Select(r => new
                 {
                     r.ReservationId,
                     GuestFullName = r.guest != null
                         ? r.guest.FirstName + " " + r.guest.LastName
                         : "Unknown Guest",

                     RoomNumber = r.rooms != null
                         ? r.rooms.RoomNumber ?? "Unknown"
                         : "Unknown Room",

                     Currency = r.rooms != null && r.rooms.roomType != null
                         ? r.rooms.roomType.Currency
                         : r.Currency, // fallback to reservation currency

                     TotalPrice = r.rooms != null && r.rooms.roomType != null
                         ? r.rooms.roomType.PricePerNight
                         : r.TotalPrice ?? 0
                 })
                 .AsEnumerable()
                 .Select(r => new
                 {
                     r.ReservationId,
                     r.GuestFullName,
                     r.RoomNumber,
                     DisplayTotal = r.Currency == "USD"
                         ? $"${r.TotalPrice:N2}"
                         : $"៛{r.TotalPrice:N0}",
                     r.Currency
                 })
                 .ToList();

            ViewBag.Reservations = reservations;


            ModelState.AddModelError("", "Delete operation failed. Please try again.");
            return View("Delete", delete);
        }

        [HttpGet]
        public async Task<IActionResult> GetTotalRevenue()
        {
            return View("GetTotalRevenue");
        }

        private async Task LoadViewBagsAsync(int? selectedPaymentMethodId = null)
        {
            var paymentMethods = await _paymethod.GetAllPaymentMethods(default);
            ViewBag.PaymentMethods = new SelectList(paymentMethods, "PaymentMethodId", "Name", selectedPaymentMethodId);

            var reservations = _context.Reservations
                  .Select(r => new
                  {
                      r.ReservationId,
                      GuestFullName = r.guest != null
                          ? r.guest.FirstName + " " + r.guest.LastName
                          : "Unknown Guest",

                      RoomNumber = r.rooms != null
                          ? r.rooms.RoomNumber ?? "Unknown"
                          : "Unknown Room",

                      Currency = r.rooms != null && r.rooms.roomType != null
                          ? r.rooms.roomType.Currency
                          : r.Currency, // fallback to reservation currency

                      TotalPrice = r.rooms != null && r.rooms.roomType != null
                          ? r.rooms.roomType.PricePerNight
                          : r.TotalPrice ?? 0
                  })
                  .AsEnumerable()
                  .Select(r => new
                  {
                      r.ReservationId,
                      r.GuestFullName,
                      r.RoomNumber,
                      DisplayTotal = r.Currency == "USD"
                          ? $"${r.TotalPrice:N2}"
                          : $"៛{r.TotalPrice:N0}",
                      r.Currency
                  })
                  .ToList();

            ViewBag.Reservations = reservations;

        }
        private async Task TotalPay()
        {

            var pendingCount = await _service.GetPaymentsCountByStatusAsync("Pending", default);
            ViewBag.PendingPayments = pendingCount;


            var totalUsd = await _service.GetAllTotalPayment("USD", default);
            var totalKhr = await _service.GetAllTotalPayment("KHR", default);


            ViewBag.TotalUsd = totalUsd.Currency == "USD"
                ? $"${totalUsd.TotalAmount:N2}"
                : totalUsd.TotalAmount.ToString("N2");

            ViewBag.TotalKhr = totalKhr.Currency == "KHR"
                ? $"៛{totalKhr.TotalAmount:N0}"
                : totalKhr.TotalAmount.ToString("N0");
        }



        //private async Task LoadRoom()
        //{
        //    var reservations = await _context.Reservations
        //        .Include(r => r.guest)
        //        .Include(r => r.rooms)
        //            .ThenInclude(rm => rm!.roomType)
        //        .Include(r => r.Payment) // ✅ Include Payment explicitly
        //        .Where(r => r.Payment == null || r.Payment.PaymentStatus != "Completed")
        //        .ToListAsync();

        //    var grouped = reservations.Select(r =>
        //    {
        //        var guestName = r.guest != null
        //            ? $"{r.guest.FirstName} {r.guest.LastName}"
        //            : "Unknown Guest";

        //        var roomNumber = r.rooms?.RoomNumber ?? "Unknown Room";
        //        var totalAmount = r.rooms?.roomType?.PricePerNight ?? 0;
        //        var currency = r.rooms?.roomType?.Currency ?? r.Currency ?? "USD";

        //        var displayTotal = currency == "USD"
        //            ? $"${totalAmount:N2}"
        //            : $"៛{totalAmount:N0}";

        //        return new
        //        {
        //            r.ReservationId,
        //            GuestFullName = guestName,
        //            RoomNumber = roomNumber,
        //            DisplayTotal = displayTotal,
        //            TotalAmount = totalAmount,
        //            Currency = currency
        //        };
        //    }).ToList();

        //    ViewBag.Reservations = grouped;
        //}
        private async Task LoadRoom()
        {
            var reservations = await _context.Reservations
                .Include(r => r.guest)
                .Include(r => r.rooms)
                    .ThenInclude(rm => rm!.roomType)
                .Include(r => r.ReservationServices) // ✅ include services
                .Include(r => r.Payment)
                .Where(r => r.Payment == null || r.Payment.PaymentStatus != "Completed")
                .ToListAsync();

            var grouped = reservations.Select(r =>
            {
                var guestName = r.guest != null
                    ? $"{r.guest.FirstName} {r.guest.LastName}"
                    : "Unknown Guest";

                var roomNumber = r.rooms?.RoomNumber ?? "Unknown Room";

                // 🧮 Get room + services total
                var roomPrice = r.rooms?.roomType?.PricePerNight ?? 0;
                var serviceTotal = r.ReservationServices?.Sum(s => s.TotalPrice ?? 0) ?? 0;
                var totalAmount = roomPrice + serviceTotal;

                var currency = r.rooms?.roomType?.Currency ?? r.Currency ?? "USD";

                // 💵 Display format
                var displayTotal = currency == "USD"
                    ? $"${totalAmount:N2}"
                    : $"៛{totalAmount:N0}";
                var displayTotalKhr = currency == "KHR" ? $"៛{totalAmount:N0}" : $"${totalAmount:N2}";

                return new
                {
                    r.ReservationId,
                    GuestFullName = guestName,
                    RoomNumber = roomNumber,
                    DisplayTotal = displayTotal,
                    DiplayTotal = displayTotalKhr,
                    TotalAmount = totalAmount,
                    Currency = currency
                };
            }).ToList();

            ViewBag.Reservations = grouped;
        }



    }
}
