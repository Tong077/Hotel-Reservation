using H_application.DTOs.RoomTypeDto;
using H_application.Service;
using Microsoft.AspNetCore.Mvc;

namespace H_Reservation.Controllers
{
    public class RoomTypeController : Controller
    {
        private readonly IRoomTypeService _service;

        public RoomTypeController(IRoomTypeService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _service.GetAllRoomTypesAsync(default);
            return View("Index", result);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View("Create");
        }
        [HttpPost]
        public async Task<IActionResult> Store(RoomTypeDtoCreate roomtype)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View("Create", roomtype);
                }
                var result = await _service.CreateRoomTypeAsync(roomtype, default);
                if (result)
                {
                    return RedirectToAction("Index");
                }
                return View("Create", roomtype);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View("Create", roomtype);
            }
        }
        [HttpGet]
        public async Task <IActionResult> Edit(int Id)
        {
            var rp = await _service.GetRoomTypeByIdAsync(Id, default);
           if(rp == null)
            {
                return View("Edit", Id);
            }
            return View("Edit", rp);
           
        }
        [HttpPost]
        public async Task<IActionResult> Update(RoomTypeDtoUpdate roomtype)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View("Edit", roomtype);
                }
                var result = await _service.UpdateRoomTypeAsync(roomtype, default);
                if (result)
                {
                    return RedirectToAction("Index");
                }
                return View("Edit", roomtype);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View("Edit", roomtype);
            }
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int Id)
        {
            var rp = await _service.GetRoomTypeByIdAsync(Id, default);
            if (rp == null)
            {
                return NotFound();
            }
            return View("Delete", rp);

        }
        [HttpPost]
        public async Task<IActionResult> Destroy(RoomTypeDtoUpdate roomtype)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View("Delete", roomtype);
                }
                var result = await _service.DeleteRoomTypeAsync(roomtype, default);
                if (result)
                {
                    return RedirectToAction("Index");
                }
                return View("Delete", roomtype);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View("Delete", roomtype);
            }
        }

    }
}
