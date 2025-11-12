using H_application.DTOs.SystemSettingsDto;
using H_application.Service;
using Microsoft.AspNetCore.Mvc;

namespace H_Reservation.Controllers
{
    public class SystemSettingsController : Controller
    {
        private readonly ISystemSettingsService _systemSetting;

        public SystemSettingsController(ISystemSettingsService systemSetting)
        {
            _systemSetting = systemSetting;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _systemSetting.GetAllSystemSetting(default);
            return View("Index", result);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View("Create");
        }
        [HttpPost]
        public async Task<IActionResult> Store(SystemSettingsDtoCreate dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View("Create", dto);
                }
                var result = await _systemSetting.CreateSetting(dto);
                if (result)
                {
                    return RedirectToAction("Index");
                }

                // Add this:
                ModelState.AddModelError("Key", "This key already exists. Please use another key.");
                return View("Create", dto);

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Unknow", ex.Message);
                return View("Create", dto);
            }
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int Id)
        {
            var get = await _systemSetting.GetSystemSettingById(Id);

            if (get == null)
                return NotFound();

            return View("Edit", get);

        }
        [HttpPost]
        public async Task<IActionResult> Update(SystemSettingsDtoUpdate dto)
        {
            if (!ModelState.IsValid)
            {
                return View("Edit");
            }
            var result = await _systemSetting.UpdateSetting(dto);
            if (result)
            {
                return RedirectToAction("Index");
            }
            return View("Edit", dto);
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int Id)
        {
            var get = await _systemSetting.GetSystemSettingById(Id);

            if (get == null)
                return NotFound();

            return View("Delete", get);

        }
        [HttpPost]
        public async Task<IActionResult> Destroy(SystemSettingsDtoUpdate dto)
        {
            if (!ModelState.IsValid)
            {
                return View("Delete");
            }
            var result = await _systemSetting.DeleteSetting(dto);
            if (result)
            {
                return RedirectToAction("Index");
            }
            return View("Delete", dto);
        }
    }
}
