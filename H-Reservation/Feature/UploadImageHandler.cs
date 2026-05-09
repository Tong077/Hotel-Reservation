
namespace H_Reservation.Feature
{
    public class UploadImageHandler : IImageUploadsService
    {
        private readonly IWebHostEnvironment _env;

        public UploadImageHandler(IWebHostEnvironment env)
        {
            _env = env;
        }
        public async Task<string?> UploadsAsynce(IFormFile? file, string folder, string? oldImage = null)
        {
            if (file == null || file.Length == 0)
            {
                return oldImage;
            }
            var uploadFolder = Path.Combine(_env.WebRootPath, folder);
            if (!Directory.Exists(uploadFolder))
            {
                Directory.CreateDirectory(uploadFolder);
            }
            if (!string.IsNullOrEmpty(oldImage))
            {
                var oldPath = Path.Combine(uploadFolder, oldImage);
                if (File.Exists(oldPath))
                {
                    File.Delete(oldPath);
                }

            }
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var filePath = Path.Combine(uploadFolder, fileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }


            return fileName;

        }
    }
}
