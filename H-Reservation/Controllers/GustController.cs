using H_application.DTOs.GuestDto;
using H_application.Error;
using H_Application.Service;
using H_Domain.DataContext;
using H_Domain.Models;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace H_Reservation.Controllers
{
    public class GustController : Controller
    {
        private readonly IGustService _guest;
        private readonly EntityContext _context;
        public GustController(IGustService guest, EntityContext context)
        {
            _context = context;
            _guest = guest;

        }
        public async Task<IActionResult> Index()
        {
            var result = await _guest.GetAllAsync(default);
            return View("Index", result);
        }



        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Store(GuestDtoCreate dtoCreate, CancellationToken cancellation)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData["toastr-type"] = "error";
                    TempData["toastr-message"] = "Guest with same Email or Phone already exists...!";
                    return View("Create", dtoCreate);
                }

                var result = await _guest.guestCreateAsync(dtoCreate, cancellation);

                if (result)
                {
                    TempData["toastr-type"] = "success";
                    TempData["toastr-message"] = "Guest Has Been Create Success Fully...!";
                    return RedirectToAction("Index");
                }

                
                ModelState.AddModelError(string.Empty, "Guest with same Email or Phone already exists.");
                return View("Create", dtoCreate);
            }
            catch (DbUpdateException ex)
            {
                // Handle DB constraint error (unique key violation)
                ModelState.AddModelError(string.Empty, "Duplicate record detected. Please check Email or Phone.");
                return View("Create", dtoCreate);
            }

        }

        [HttpGet]
        public async Task<IActionResult> Edit(int Id)
        {
            var result = await _guest.GetById(Id);
            if (result == null)
                return NotFound();
            return View(result);
        }
        [HttpPost]
        public async Task<IActionResult> Update(GuestDtoUpdate guestDtoUpdate, CancellationToken cancellation)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData["toastr-type"] = "success";
                    TempData["toastr-message"] = "Guest Has Been updated Success Fully...!";
                    return View("Edit", guestDtoUpdate);
                }
                var result = await _guest.guestUpdateAsync(guestDtoUpdate, cancellation);
                if (result)
                {
                    return RedirectToAction("Index");
                }
                return View("Edit", guestDtoUpdate);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Unexpected error: " + ex.Message);
                return View("Edit", guestDtoUpdate);
            }

        }
        [HttpGet]
        public async Task<IActionResult> Delete(int Id)
        {
            var result = await _guest.GetById(Id);
            if (result == null)
                return NotFound();
            return View(result);
        }
        [HttpPost]
        public async Task<IActionResult> Destroy(GuestDtoUpdate guestDtoUpdate, CancellationToken cancellation)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View("Delete", guestDtoUpdate);
                }
                var result = await _guest.guestDeleteAsync(guestDtoUpdate, cancellation);
                if (result)
                {
                    TempData["toastr-type"] = "success";
                    TempData["toastr-message"] = "Guest Has Been Delete Success Fully...!";
                    return RedirectToAction("Index");
                }
                return View("Delete", guestDtoUpdate);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Unexpected error: " + ex.Message);
                return View("Delete", guestDtoUpdate);
            }

        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> _Store([FromForm] GuestDtoCreate dtoCreate, CancellationToken cancellation)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState
                        .Where(x => x.Value!.Errors.Any())
                        .ToDictionary(
                            kvp => kvp.Key,
                            kvp => kvp.Value!.Errors.Select(e => e.ErrorMessage).FirstOrDefault()
                        );

                    return Json(new { success = false, errors });
                }

                var (success, newGuestId, errorType) = await _guest.guestCreatsAsync(dtoCreate, cancellation);

                if (!success)
                {
                    var errors = new Dictionary<string, string>();
                    if (errorType == "Email")
                    {
                        errors["Email"] = "An guest with this email already exists.";
                    }
                    else if (errorType == "Phone")
                    {
                        errors["Phone"] = "An guest with this phone number already exists.";
                    }
                    
                    return Json(new { success = false, errors });
                }

                // Fetch the new guest for additional details if needed
                var newGuest = await _context.Guests.FindAsync(newGuestId);

                return Json(new
                {
                    success = true,
                    newGuest = new
                    {
                        guestId = newGuest!.GuestId,
                        fullName = $"{newGuest.FirstName} {newGuest.LastName}"
                    }
                });
            }
            catch (Exception ex)
            {
                // Return consistent JSON error for better client handling
                return Json(new { success = false, message = $"An unexpected error occurred: {ex.Message}" });
            }
        }



    }
    
}



