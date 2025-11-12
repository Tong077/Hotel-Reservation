using H_application.DTOs.RoomDto;
using H_application.Service;
using H_Domain.DataContext;
using H_Domain.Models;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace H_application.Repository
{
    public class RoomRepository : IRoomService
    {
        private readonly EntityContext _context;
        public RoomRepository(EntityContext context)
        {
            this._context = context;
        }

        public async Task<bool> CreateRoomAsync(RoomDtoCreate room, CancellationToken cancellationToken)
        {
            var entity = room.Adapt<Room>();
            await _context.Rooms.AddAsync(entity, cancellationToken);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteRoomAsync(RoomDtoUpdate room, CancellationToken cancellation)
        {
            var entity = room.Adapt<Room>();
            _context.Rooms.Remove(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<RoomResponse>> GetAllRoomAsync(CancellationToken cancellationToken)
        {
            var result = await _context.Rooms
                .Include(type => type.roomType)
                .Include(h => h.hotel)
                .AsNoTracking().ToListAsync();

            var entity = result.Select(r => new RoomResponse
            {
                RoomId = r.RoomId,
                RoomNumber = r.RoomNumber,
                RoomTypeId = r.RoomTypeId,
                RoomTypeName = r.roomType != null ? r.roomType.Name : string.Empty,
                RoomPrice = r.roomType != null ? r.roomType.PricePerNight : 0,
                RoomCurrency = r.roomType.Currency,
                Status = r.Status,
                Images = r.Images,
                HotelId = r.HotelId,
                HotelName = r.hotel != null ? r.hotel.Name : string.Empty

            });
            return entity;
        }

        public async Task<List<RoomResponse>> GetAvailableRoomsAsync(CancellationToken cancellationToken)
        {
            var allowedStatuses = new[] { "Available", "Occupied", "Reserved", "Cleaning" };

            return await _context.Rooms
                .Include(r => r.roomType)
                .Where(r => allowedStatuses.Contains(r.Status))
                .Select(r => new RoomResponse
                {
                    RoomId = r.RoomId,
                    RoomNumber = r.RoomNumber,
                    RoomPrice = r.roomType!.PricePerNight,
                    Currency = r.roomType.Currency,
                    RoomTypeName = r.roomType != null ? r.roomType.Name : string.Empty,
                    Status = r.Status
                })
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }

        public async Task<List<RoomResponse>> GetRoomStatusRoom(CancellationToken cancellationToken)
        {
            // Fetch only rooms with valid statuses
            var rooms = await _context.Rooms
                .Include(r => r.roomType)
                .Where(r => r.Status!.Contains(r.Status))
                .OrderBy(r => r.Status)       // Optional: order by status
                .ThenBy(r => r.RoomNumber)   // Optional: order by room number
                .Select(r => new RoomResponse
                {
                    RoomId = r.RoomId,
                    RoomNumber = r.RoomNumber,
                    RoomTypeName = r.roomType != null ? r.roomType.Name : string.Empty,
                    Status = r.Status
                })
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            return rooms;
        }

        public async Task<RoomDtoUpdate> GetRoomByIdAsync(int Id, CancellationToken cancellationToken)
        {
            var result = await _context.Rooms
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.RoomId == Id, cancellationToken);

            var entity = result.Adapt<RoomDtoUpdate>();
            return entity;
        }
        public async Task<bool> UpdateRoomAsync(RoomDtoUpdate room, CancellationToken cancellationToken)
        {

            var entity = room.Adapt<Room>();
            _context.Rooms.Update(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<List<RoomResponse>> GetAvailableStatusRoom(CancellationToken cancellationToken)
        {
            return await _context.Rooms
                .Include(r => r.roomType)
                .Where(r => r.Status == "Available")
                .Select(r => new RoomResponse
                {
                    RoomId = r.RoomId,
                    RoomNumber = r.RoomNumber,
                    RoomPrice = r.roomType!.PricePerNight,
                    Currency = r.roomType.Currency,
                    RoomTypeName = r.roomType != null ? r.roomType.Name : string.Empty,
                    Status = r.Status
                })
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }
    }
}
