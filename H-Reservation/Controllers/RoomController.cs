using H_application.DTOs.RoomDto;
using H_application.Service;
using H_Domain.DataContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace H_Reservation.Controllers
{
    public class RoomController : Controller
    {
        private readonly IRoomService _roomService;
        private readonly IWebHostEnvironment _enviroment;
        private readonly IRoomTypeService _type;
        private readonly IHotelServicecs _hetel;
        private readonly EntityContext _context;

        public RoomController(IRoomService roomService, IWebHostEnvironment enviroment, IHotelServicecs hetel, IRoomTypeService type, EntityContext context)
        {
            _roomService = roomService;
            _enviroment = enviroment;
            _hetel = hetel;
            _type = type;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _roomService.GetAllRoomAsync(default);
            return View("Index", result);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var type = await _type.GetAllRoomTypesAsync(default);
            ViewBag.RoomType = new SelectList(type, "RoomTypeId", "Name");
            var hotel = await _hetel.GetAllHotelAsync(default);
            ViewBag.Hotels = new SelectList(hotel, "HotelId", "Name");
            return View("Create");
        }

        [HttpPost]
        public async Task<IActionResult> Store(RoomDtoCreate room, List<IFormFile?> file)
        {
            try
            {

                var existingRoom = await _context.Rooms
                    .FirstOrDefaultAsync(r => r.RoomNumber == room.RoomNumber);
                if (existingRoom != null)
                {
                    ModelState.Remove("RoomNumber");
                    ModelState.AddModelError("RoomNumber", "Room number already exists.");
                }

                var fileNames = new List<string>();
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                var maxFileSize = 2 * 1024 * 1024; // 2 MB

                if (file != null && file.Count > 0)
                {
                    var directPath = Path.Combine(_enviroment.WebRootPath, "images/rooms");
                    if (!Directory.Exists(directPath))
                        Directory.CreateDirectory(directPath);

                    foreach (var f in file)
                    {
                        if (f.Length > 0)
                        {
                            var ext = Path.GetExtension(f.FileName).ToLower();

                            // Validate file type
                            if (!allowedExtensions.Contains(ext))
                            {
                                ModelState.AddModelError("Images", $"File '{f.FileName}' is not an allowed image type.");
                                continue;
                            }

                            // Validate file size
                            if (f.Length > maxFileSize)
                            {
                                ModelState.AddModelError("Images", $"File '{f.FileName}' exceeds 2 MB.");
                                continue;
                            }

                            string fileName = $"{Guid.NewGuid()}{ext}";
                            string filePath = Path.Combine(directPath, fileName);

                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                await f.CopyToAsync(stream);
                            }

                            fileNames.Add(fileName);
                        }
                    }

                    room.Images = string.Join(",", fileNames);
                }

                if (!ModelState.IsValid)
                {

                    var type = await _type.GetAllRoomTypesAsync(default);
                    ViewBag.RoomType = new SelectList(type, "RoomTypeId", "Name");

                    var hotel = await _hetel.GetAllHotelAsync(default);
                    ViewBag.Hotels = new SelectList(hotel, "HotelId", "Name");

                    return View("Create", room);
                }

                var result = await _roomService.CreateRoomAsync(room, default);
                if (result)
                {
                    return RedirectToAction("Index");
                }
                return View("Create", room);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View("Create", room);
            }
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var result = await _roomService.GetRoomByIdAsync(id, default);

            var type = await _type.GetAllRoomTypesAsync(default);
            ViewBag.RoomType = new SelectList(type, "RoomTypeId", "Name");

            var hotel = await _hetel.GetAllHotelAsync(default);
            ViewBag.Hotels = new SelectList(hotel, "HotelId", "Name");

            return View("Edit", result);
        }

        [HttpPost]
        public async Task<IActionResult> Update(RoomDtoUpdate room)
        {
            try
            {
                var existingRoom = await _context.Rooms.FindAsync(room.RoomId);
                if (existingRoom == null)
                    return NotFound();

                var currentImages = !string.IsNullOrEmpty(existingRoom.Images)
                    ? existingRoom.Images.Split(',').ToList()
                    : new List<string>();

                // Handle removeImage checkboxes
                var removeImages = Request.Form["removeImage"].ToList();
                currentImages = currentImages.Where(img => !removeImages.Contains(img)).ToList();

                // Handle replacement images
                for (int i = 0; i < currentImages.Count; i++)
                {
                    var fileInputName = $"replaceImage_{i}";
                    var files = Request.Form.Files.GetFiles(fileInputName);
                    if (files.Count > 0)
                    {
                        var file = files[0]; // Only one file per replace input
                        if (file != null && file.Length > 0)
                        {
                            var ext = Path.GetExtension(file.FileName).ToLower();
                            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                            var maxFileSize = 2 * 1024 * 1024; // 2 MB

                            if (!allowedExtensions.Contains(ext))
                            {
                                ModelState.AddModelError("Images", $"File '{file.FileName}' is not an allowed image type.");
                                continue;
                            }
                            if (file.Length > maxFileSize)
                            {
                                ModelState.AddModelError("Images", $"File '{file.FileName}' exceeds 2 MB.");
                                continue;
                            }

                            // Save new file
                            var directPath = Path.Combine(_enviroment.WebRootPath, "images/rooms");
                            if (!Directory.Exists(directPath))
                                Directory.CreateDirectory(directPath);

                            var newFileName = $"{Guid.NewGuid()}{ext}";
                            var filePath = Path.Combine(directPath, newFileName);
                            using (var stream = new FileStream(filePath, FileMode.Create))
                                await file.CopyToAsync(stream);

                            // Replace old image with new one
                            currentImages[i] = newFileName;
                        }
                    }
                }

                // Handle additional uploaded images from the main multiple file input
                var newFiles = Request.Form.Files.GetFiles("file");
                if (newFiles != null && newFiles.Count > 0)
                {
                    var directPath = Path.Combine(_enviroment.WebRootPath, "images/rooms");
                    if (!Directory.Exists(directPath))
                        Directory.CreateDirectory(directPath);

                    foreach (var f in newFiles)
                    {
                        if (f != null && f.Length > 0)
                        {
                            var ext = Path.GetExtension(f.FileName).ToLower();
                            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                            var maxFileSize = 2 * 1024 * 1024; // 2 MB

                            if (!allowedExtensions.Contains(ext))
                            {
                                ModelState.AddModelError("Images", $"File '{f.FileName}' is not an allowed image type.");
                                continue;
                            }
                            if (f.Length > maxFileSize)
                            {
                                ModelState.AddModelError("Images", $"File '{f.FileName}' exceeds 2 MB.");
                                continue;
                            }

                            var fileName = $"{Guid.NewGuid()}{ext}";
                            var filePath = Path.Combine(directPath, fileName);
                            using (var stream = new FileStream(filePath, FileMode.Create))
                                await f.CopyToAsync(stream);

                            currentImages.Add(fileName);
                        }
                    }
                }

                // Update room properties
                existingRoom.RoomNumber = room.RoomNumber;
                existingRoom.RoomTypeId = room.RoomTypeId;
                existingRoom.HotelId = room.HotelId;
                existingRoom.Status = room.Status;
                existingRoom.Images = string.Join(",", currentImages);

                if (!ModelState.IsValid)
                {
                    ViewBag.RoomType = new SelectList(await _type.GetAllRoomTypesAsync(default), "RoomTypeId", "Name");
                    ViewBag.Hotels = new SelectList(await _hetel.GetAllHotelAsync(default), "HotelId", "Name");
                    return View("Edit", room);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                ViewBag.RoomType = new SelectList(await _type.GetAllRoomTypesAsync(default), "RoomTypeId", "Name");
                ViewBag.Hotels = new SelectList(await _hetel.GetAllHotelAsync(default), "HotelId", "Name");
                return View("Edit", room);
            }
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _roomService.GetRoomByIdAsync(id, default);

            var type = await _type.GetAllRoomTypesAsync(default);
            ViewBag.RoomType = new SelectList(type, "RoomTypeId", "Name");

            var hotel = await _hetel.GetAllHotelAsync(default);
            ViewBag.Hotels = new SelectList(hotel, "HotelId", "Name");

            return View("Delete", result);
        }
        [HttpPost]
        public async Task<IActionResult> Destroy(RoomDtoUpdate room)
        {
            try
            {
                var type = await _type.GetAllRoomTypesAsync(default);
                ViewBag.RoomType = new SelectList(type, "RoomTypeId", "Name");

                var hotel = await _hetel.GetAllHotelAsync(default);
                ViewBag.Hotels = new SelectList(hotel, "HotelId", "Name");

                if (!ModelState.IsValid)
                {
                    return View("Delete", room);
                }
                var result = await _roomService.DeleteRoomAsync(room, default);
                if (result)
                {
                    return RedirectToAction("Index");
                }
                return View("Delete", room);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View("Delete", room);
            }


        }
        [HttpPost]
        public async Task<IActionResult> BulkUpdateStatus(List<int> selectedRoomIds, string status, CancellationToken cancellationToken)
        {
            // Fix: Use !selectedRoomIds.Any()
            if (selectedRoomIds == null || !selectedRoomIds.Any() || string.IsNullOrEmpty(status))
            {
                TempData["Error"] = "Please select at least one room and a status.";
                return RedirectToAction(nameof(Index));
            }

            var rooms = await _context.Rooms
                .Where(r => selectedRoomIds.Contains(r.RoomId))
                .ToListAsync(cancellationToken);

            if (!rooms.Any())
            {
                TempData["Error"] = "No matching rooms found.";
                return RedirectToAction(nameof(Index));
            }

            foreach (var room in rooms)
            {
                room.Status = status;
            }

            await _context.SaveChangesAsync(cancellationToken);

            TempData["Success"] = $"{rooms.Count} room(s) updated to '{status}' successfully.";
            return RedirectToAction(nameof(Index));
        }

    }
}
