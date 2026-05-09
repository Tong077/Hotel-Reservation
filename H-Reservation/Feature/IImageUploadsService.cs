namespace H_Reservation.Feature
{
    public interface IImageUploadsService
    {
        Task<string?> UploadsAsynce(IFormFile? file, string folder, string? oldImage = null);
    }
}
