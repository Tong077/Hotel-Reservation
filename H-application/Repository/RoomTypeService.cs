using H_application.DTOs.RoomTypeDto;
using H_application.Service;
using H_Domain.DataContext;
using H_Domain.Models;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace H_application.Repository
{
    public class RoomTypeService : IRoomTypeService
    {
        private readonly EntityContext _conext;
        public RoomTypeService(EntityContext context)
        {
            _conext = context;
        }
        public async Task<bool> CreateRoomTypeAsync(RoomTypeDtoCreate roomtype, CancellationToken cancellationToken = default)
        {
            var entity = roomtype.Adapt<RoomType>();
            await _conext.RoomTypes.AddAsync(entity);
            return await _conext.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteRoomTypeAsync(RoomTypeDtoUpdate roomtype, CancellationToken cancellation = default)
        {
            var entity = roomtype.Adapt<RoomType>();
            _conext.RoomTypes.Remove(entity);
            return await _conext.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<RoomTypeResponse>> GetAllRoomTypesAsync(string filter, CancellationToken cancellationToken = default)
        {
            var result = await _conext.RoomTypes
                .AsNoTracking().ToListAsync();
            var entity = result.Adapt<List<RoomTypeResponse>>();
            return entity;

        }

        public async Task<RoomTypeDtoUpdate> GetRoomTypeByIdAsync(int Id, CancellationToken cancellationToken = default)
        {
            var result = await _conext.RoomTypes
                .FirstOrDefaultAsync(r => r.RoomTypeId == Id);
            var entity = result?.Adapt<RoomTypeDtoUpdate>();
            return entity!;
        }

        public async Task<bool> UpdateRoomTypeAsync(RoomTypeDtoUpdate roomtype, CancellationToken cancellationToken = default)
        {
            var entity = roomtype.Adapt<RoomType>();
            _conext.RoomTypes.Update(entity);
            return await _conext.SaveChangesAsync() > 0;
        }
    }
}
