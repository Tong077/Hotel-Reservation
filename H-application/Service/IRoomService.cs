using H_application.DTOs.RoomDto;

namespace H_application.Service
{
    public interface IRoomService
    {
        Task<bool> CreateRoomAsync(RoomDtoCreate room, CancellationToken cancellationToken);
        Task<bool> UpdateRoomAsync(RoomDtoUpdate room, CancellationToken cancellationToken);
        Task<bool> DeleteRoomAsync(RoomDtoUpdate room, CancellationToken cancellation);
        Task<RoomDtoUpdate> GetRoomByIdAsync(int Id, CancellationToken cancellationToken);
        Task<IEnumerable<RoomResponse>> GetAllRoomAsync(CancellationToken cancellationToken = default);

        Task<List<RoomResponse>> GetAvailableRoomsAsync(CancellationToken cancellationToken);
        Task<List<RoomResponse>> GetRoomStatusRoom(CancellationToken cancellationToken);

        Task<List<RoomResponse>> GetAvailableStatusRoom(CancellationToken cancellationToken);
    }
}
