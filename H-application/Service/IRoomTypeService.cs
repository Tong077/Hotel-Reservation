using H_application.DTOs.RoomTypeDto;

namespace H_application.Service
{
    public interface IRoomTypeService
    {
        Task<bool> CreateRoomTypeAsync(RoomTypeDtoCreate roomtype, CancellationToken cancellationToken = default);
        Task<bool> UpdateRoomTypeAsync(RoomTypeDtoUpdate roomtype, CancellationToken cancellationToken = default);
        Task<bool> DeleteRoomTypeAsync(RoomTypeDtoUpdate roomtype, CancellationToken cancellation = default);

        Task<RoomTypeDtoUpdate> GetRoomTypeByIdAsync(int Id, CancellationToken cancellationToken = default);
        Task<IEnumerable<RoomTypeResponse>> GetAllRoomTypesAsync(string filter, CancellationToken cancellationToken = default);

    }
}
