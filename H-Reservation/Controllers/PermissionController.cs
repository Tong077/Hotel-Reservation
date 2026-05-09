using H_Application.DTOs.PermissionDto;
using H_Application.Service;
using Microsoft.AspNetCore.Mvc;

namespace H_Reservation.Controllers
{
    public class PermissionController : Controller
    {
        private readonly IPermissionService _service;
        public PermissionController(IPermissionService service)
        {
            _service = service;
        }   

        public async Task <IActionResult> Index()
        {
            var pr  = await _service.GetallPermission();

            return View("index",pr);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View("Create");
        }
        [HttpPost]
        public async Task<IActionResult> Store(PermissionDtoCreate dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(dto);
                }
                var pr = await _service.CraetePermission(dto);
                if (pr)
                {
                    return RedirectToAction("Index");
                }
                return View(dto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int Id)
        {
            var pr = await _service.GetById(Id);
            if(pr != null)
            {
                return View("Edit",pr);
            }
            return View("Edit");
        }
        [HttpPost]
        public async Task<IActionResult> Update(PermissionDtoUpdate dto)
        {
            if (!ModelState.IsValid)
            {
                return View("Edit", dto);
            }
            var pr = await _service.UpdatePermission(dto);
            if (pr)
            {
                return RedirectToAction("Index");
            }
            return View("Index");
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int Id)
        {
            var pr = await _service.GetById(Id);
            if (pr != null)
            {
                return View("Delete", pr);
            }
            return View("Delete");
        }
        [HttpPost]
        public async Task<IActionResult> Destroy(PermissionDtoUpdate dto)
        {
            if (!ModelState.IsValid)
            {
                return View("Delete", dto);
            }
            var pr = await _service.DeletePermisson(dto);
            if (pr)
            {
                return RedirectToAction("Index");
            }
            return View("Delete");
        }
    }
}
