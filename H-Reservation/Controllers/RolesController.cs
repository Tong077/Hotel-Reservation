using H_Application.DTOs.RoleDto;
using H_Application.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace H_Reservation.Controllers
{
    public class RolesController : Controller
    {
        private readonly IRoleService roleService;
        private readonly IPermissionService _psermission;
        public RolesController(IRoleService roleService, IPermissionService psermission)
        {
            this.roleService = roleService;
            _psermission = psermission;
        }

        public async Task<IActionResult> Index()
        {
            var roles = await roleService.GetAllRoles();

            return View("Index", roles);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var permissions = await _psermission.GetallPermission();
            ViewBag.Permissions = new SelectList(permissions, "Id", "Name");
            return View("Create");
        }
        [HttpPost]
        public async Task<IActionResult> Store(RoleCreateDto roleCreateDto)
        {
            if (!ModelState.IsValid)
            {
                var permissions = await _psermission.GetallPermission();
                ViewBag.Permissions = new SelectList(permissions, "Id", "Name");
                return View("Create", roleCreateDto);
            }
            var roles = await roleService.CreateRole(roleCreateDto);
            if (roles)
            {
                return RedirectToAction("Index");
            }
            return View("Create", roleCreateDto);


        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var role = await roleService.GetRoleById(id);
            if (role == null)
            {
                return NotFound();
            }
            var permissions = await _psermission.GetallPermission();
            ViewBag.Permissions = new SelectList(permissions, "Id", "Name");
            return View("Edit", role);
        }
        [HttpPost]
        public async Task<IActionResult> Update(RoleUpdateDto roleUpdateDto)
        {
            if (!ModelState.IsValid)
            {
                var permissions = await _psermission.GetallPermission();
                ViewBag.Permissions = new SelectList(permissions, "Id", "Name");
                return View("Edit", roleUpdateDto);
            }
            var roles = await roleService.UpdateRole(roleUpdateDto);
            if (roles)
            {
                return RedirectToAction("Index");
            }
            return View("Edit", roleUpdateDto);
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellation = default)
        {
            var role = await roleService.GetRoleById(id, cancellation);

            if (role == null)
                return NotFound();

            return View(role);
        }

        [HttpPost]
     
        public async Task<IActionResult> DeleteCo(RoleUpdateDto roleUpdateDto, CancellationToken cancellation = default)
        {
            if (!ModelState.IsValid)
            {
                var role = await roleService.GetRoleById(roleUpdateDto.Id, cancellation);
                if (role == null)
                    return NotFound();
                return View("Delete", role);
            }
            var result = await roleService.DeleteRole(roleUpdateDto, cancellation);
            if (result)
            {
                return RedirectToAction("Index");
            }
            return View("Delete", roleUpdateDto);
        }
    }
}
