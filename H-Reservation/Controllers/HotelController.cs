using H_application.DTOs.HotelDto;
using H_application.Service;
using H_Domain.DataContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace H_Reservation.Controllers
{
    public class HotelController : Controller
    {
        private readonly IHotelServicecs _service;
        private readonly EntityContext _context;
        public HotelController(IHotelServicecs service, EntityContext context)
        {
            _service = service;
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var hotel = await _service.GetAllHotelAsync(default);
            return View("Index", hotel);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View("Create");
        }
        [HttpPost]
        public async Task<IActionResult> Store(HotelDtoCreate hotelDto)
        {
            try
            {
                var exist = await _context.Hotels.AnyAsync(h => h.Email == hotelDto.Email);
                if (exist)
                {
                    ModelState.AddModelError(string.Empty, "This Email Already Exists...!");
                    return View("Create", hotelDto);
                }

                if (!ModelState.IsValid)
                {
                    TempData["toastr-type"] = "error";
                    TempData["toastr-message"] = "Hotel Can't be Create success Flully ...!";
                    return View("Create", hotelDto);
                }
                var result = await _service.CreatehotelAsync(hotelDto, default);
                if (result)
                {
                    TempData["toastr-type"] = "success";
                    TempData["toastr-message"] = "Hotel Has Been Create Success Fully...!";

                    return RedirectToAction("Index");
                }
                return View("Create", hotelDto);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                throw;
            }
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var hotel = await _service.GetHotelByIdAsync(id, default);
            if (hotel == null)
            {
                return NotFound();
            }
            return View("Edit", hotel);
        }
        [HttpPost]
        public async Task<IActionResult> Update(HotelDtoUpdate hotelDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View("Edit", hotelDto);
                }
                var result = await _service.UpdatehotelAsync(hotelDto, default);
                if (result)
                {
                    TempData["toastr-type"] = "success";
                    TempData["toastr-message"] = "Hotel Has Been Update Success Fully...!";
                    return RedirectToAction("Index");
                }
                return View("Edit", hotelDto);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                throw;
            }
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var hotel = await _service.GetHotelByIdAsync(id, default);
            if (hotel == null)
            {
                return NotFound();
            }
            return View("Delete", hotel);
        }
        [HttpPost]
        public async Task<IActionResult> Destroy(HotelDtoUpdate hotelDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData["toastr-type"] = "error";
                    TempData["toastr-message"] = "Can't Delete Hotel Information...!";
                    return View("Delete", hotelDto);
                }
                var result = await _service.DeletehotelAsync(hotelDto, default);
                if (result)
                {
                    TempData["toastr-type"] = "success";
                    TempData["toastr-message"] = "The Hotel Information Has Been Delete Success Fully...!";
                    return RedirectToAction("Index");
                }
                return View("Delete", hotelDto);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                throw;
            }
        }
    }
}
