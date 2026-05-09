using H_application.DTOs.UserDto;
using H_Domain.Models;
using H_Reservation.Feature;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace H_Reservation.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IImageUploadsService _img;

        public UserController(UserManager<ApplicationUser> userManager, IImageUploadsService img)
        {
            _userManager = userManager;
            _img = img;
        }

     
        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync();

            var userList = users.Select(u => new UserRespone
            {
                Id = u.Id.ToString(),
                Username = u.UserName,
                email = u.Email,
                phonenumber = u.PhoneNumber,
                Image = u.ImageUrl
            }).ToList();

            return View(userList);
        }
        
        [HttpGet]
        public IActionResult Create()
        {
            return View("Create");
        }
   
        [HttpPost]
        public async Task<IActionResult> Store(UserDtoCreate dto)
        {
            if (!ModelState.IsValid)
                return View("Create", dto);

            string? imageName = await _img.UploadsAsynce(dto.Image, "uploads/users");

            var user = new ApplicationUser
            {
                
                UserName = dto.Username,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                ImageUrl = imageName
            };

            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
            {
                foreach (var err in result.Errors)
                    ModelState.AddModelError("", err.Description);
                return View("Create", dto);
            }

            return RedirectToAction(nameof(Index));
        }

        
        [HttpGet]
        public async Task<IActionResult> Edit(string Id)
        {
            var user = await _userManager.FindByIdAsync(Id);
            if (user == null)
                return NotFound();

            var dto = new UserDtoUpdate
            {
                Id = user.Id.ToString(),
                Username = user.UserName,
                email = user.Email,
                phonenumber = user.PhoneNumber,
                OldImage = user.ImageUrl
            };

            return View(dto);
        }

        
        [HttpPost]
        public async Task<IActionResult> Update(UserDtoUpdate dto)
        {
            var user = await _userManager.FindByIdAsync(dto.Id);
            if (user == null)
                return NotFound();

            user.UserName = dto.Username;
            user.Email = dto.email;
            user.PhoneNumber = dto.phonenumber;
            user.ImageUrl = await _img.UploadsAsynce(dto.Image, "uploads/users", dto.OldImage);

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                foreach (var err in result.Errors)
                    ModelState.AddModelError("", err.Description);
                return View("Edit", dto);
            }

            return RedirectToAction(nameof(Index));
        }

       
        [HttpGet]
        public async Task<IActionResult> Delete(string Id)
        {
            var user = await _userManager.FindByIdAsync(Id);
            if (user == null)
                return NotFound();

            var dto = new UserRespone
            {
                Username = user.UserName,
                email = user.Email,
                phonenumber = user.PhoneNumber,
                Image = user.ImageUrl
            };

            return View(dto); 
        }

      
        [HttpPost]
        public async Task<IActionResult> Destroy(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound();

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Failed to delete user");
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
