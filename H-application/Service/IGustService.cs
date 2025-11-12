using H_application.DTOs.GuestDto;


namespace H_Application.Service
{
    public interface IGustService
    {
        Task<bool> guestCreateAsync(GuestDtoCreate create, CancellationToken cancellationToken);
        Task<bool> guestUpdateAsync(GuestDtoUpdate update, CancellationToken cancellationToken);
        Task<bool> guestDeleteAsync(GuestDtoUpdate delete, CancellationToken cancellationToken);
        Task<GuestDtoUpdate> GetById(int GuestId);

        Task<IEnumerable<GuestResponse>> GetAllAsync(CancellationToken cancellationToken);
        Task<(bool Success, int GuestId, string? ErrorType)> guestCreatsAsync(GuestDtoCreate create, CancellationToken cancellationToken);
    }
}
