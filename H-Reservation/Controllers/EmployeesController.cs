using H_application.DTOs.EmployeesDto;
using H_application.Service;
using Microsoft.AspNetCore.Mvc;

namespace H_Reservation.Controllers
{
    public class EmployeesController : Controller
    {
  
        private readonly IEmployeesService _service;
        public EmployeesController(IEmployeesService service)
        {
            _service = service;
        }
        public async Task<IActionResult> Index()
        {
            var result = await _service.GetEmployees();
            return View("Index", result);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View("Create");
        }
        [HttpPost]
        public async Task<IActionResult> Store(EmployeeDtoCreate dto)
        {

            if (!ModelState.IsValid)
            {
                return View("Create", dto);
            }
            var result = _service.CreateEmployee(dto);
            if (result != null)
            {
                return RedirectToAction("index");
            }
            return View("Edit", dto);
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int Id)
        {
            var employee = await _service.GetEmployeeById(Id);
            if (employee == null)
                return NotFound();
            return View("Edit", employee);
        }
        [HttpPost]
        public async Task<IActionResult> Update(EmployeeDtoUpdate dto)
        {
            if (!ModelState.IsValid)
            {
                return View("Edit");
            }
            var result = await _service.UpdateEmployee(dto, default);
            if (result)
            {
                return RedirectToAction("Index");
            }
            return View("Delete");
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int Id)
        {
            var employee = await _service.GetEmployeeById(Id);
            if (employee == null)
                return NotFound();
            return View("Delete", employee);
        }
        [HttpPost]
        public async Task<IActionResult> Destroy(EmployeeDtoUpdate dto)
        {
            if (!ModelState.IsValid)
            {
                return View("Delete");
            }
            var result = await _service.DeleteEmployee(dto, default);
            if (result)
            {
                return RedirectToAction("Index");
            }
            return View("Delete");
        }
    }
}
