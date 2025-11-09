using H_application.DTOs.Payment;
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

            return View("Edit", result);
        }

        [HttpPost]
        public async Task<IActionResult> Update(PaymentDtoUpdate update)
        {

            if (!ModelState.IsValid)
            {
                await LoadViewBagsAsync(update.PaymentMethodId);
                return View("Edit", update);
            }

            // ✅ Check if payment exists
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


            var result = await _service.UpdatePaymentWithInvoiceAsync(update, default);

            if (result)
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



        private async Task LoadRoom()
        {
            var reservations = await _context.Reservations
                .Include(r => r.guest)
                .Include(r => r.rooms)
                    .ThenInclude(rm => rm!.roomType)
                .Where(r => r.PaymentId == null || r.Payment!.PaymentStatus != "Completed")
                .ToListAsync();

            var grouped = reservations
                .Select(r => new
                {
                    r.ReservationId,
                    GuestFullName = r.guest != null
                        ? $"{r.guest.FirstName} {r.guest.LastName}"
                        : "Unknown Guest",
                    RoomNumber = r.rooms?.RoomNumber ?? "Unknown Room",
                    TotalAmount = r.rooms?.roomType?.PricePerNight ?? 0,
                    Currency = r.rooms?.roomType?.Currency ?? r.Currency
                })
                .Select(r => new
                {
                    r.ReservationId,
                    r.GuestFullName,
                    r.RoomNumber,
                    DisplayTotal = r.Currency == "USD"
                        ? $"${r.TotalAmount:N2}"
                        : $"៛{r.TotalAmount:N0}",
                    r.TotalAmount,
                    r.Currency
                })
                .ToList();

            ViewBag.Reservations = grouped;
        }



        //[HttpGet]
        //public async Task<IActionResult> GetReservationAmount(int reservationId, CancellationToken cancellation)
        //{
        //    var firstReservation = await _context.Reservations
        //        .Include(r => r.ReservationServices)
        //            .ThenInclude(rs => rs.Service)
        //        .Include(r => r.rooms)
        //            .ThenInclude(rm => rm.roomType)
        //        .FirstOrDefaultAsync(r => r.ReservationId == reservationId, cancellation);

        //    if (firstReservation == null)
        //        return NotFound();

        //    var guestId = firstReservation.GuestId;
        //    var checkInDate = firstReservation.CheckInDate;
        //    var checkOutDate = firstReservation.CheckOutDate;

        //    //  Get all reservations for this guest & same date range
        //    var reservations = await _context.Reservations
        //        .Include(r => r.ReservationServices)
        //            .ThenInclude(rs => rs.Service)
        //        .Include(r => r.rooms)
        //            .ThenInclude(rm => rm.roomType)
        //        .Where(r => r.GuestId == guestId &&
        //                    r.CheckInDate == checkInDate &&
        //                    r.CheckOutDate == checkOutDate)
        //        .ToListAsync(cancellation);

        //    //  Calculate total room prices
        //    decimal totalRoomPrice = reservations.Sum(r =>
        //        r.rooms?.roomType?.PricePerNight ?? 0);

        //    //  Calculate total service prices
        //    decimal totalServicePrice = reservations.Sum(r =>
        //        r.ReservationServices?.Sum(rs => rs.Service?.Price ?? 0) ?? 0);

        //    decimal totalAmount = totalRoomPrice + totalServicePrice;

        //    //  Currency should come from reservation (no symbol)
        //    string currency = firstReservation.Currency ?? firstReservation.rooms?.roomType?.Currency ?? "USD";

        //    return Json(new
        //    {
        //        totalAmount,
        //        currency
        //    });
        //}

        
    }
}
