using H_application.DTOs.HousekeepingDto;
using H_application.Service;
using H_Domain.DataContext;
using H_Domain.Models;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace H_application.Repository
{
    public class HousekeepingRepository : IHousekeepingService
    {
        private readonly EntityContext _context;

        public HousekeepingRepository(EntityContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateHouseKeeping(HousekeepingDtoCreate dto, CancellationToken cancellationToken = default)
        {
            var entity = dto.Adapt<Housekeeping>();
            await _context.Housekeeping.AddAsync(entity, cancellationToken);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteHouseKeeping(HousekeepingDtoUpdate dto, CancellationToken cancellationToken = default)
        {
            var entity = dto.Adapt<Housekeeping>();
            _context.Housekeeping.Remove(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<List<HousekeepingRresponse>> GetHouseKeepingAll(CancellationToken cancellationToken = default)
        {
            var house = await _context.Housekeeping
                .Include(h => h.Room)
                .Include(h => h.Employee)
                .Include(h => h.Room!.roomType)
                .AsNoTracking().ToListAsync();

            var entity = house.Select(s => new HousekeepingRresponse
            {
                HousekeepingId = s.HousekeepingId,
                EmployeeName = s.Employee!.FullName,
                EmployeeId = s.EmployeeId,
                RoomId = s.RoomId,
                RoomNumber = s.Room!.RoomNumber,
                RoomTypeName = s.Room.roomType!.Name,
                LastCleanedDate = s.LastCleanedDate,
                Status = s.Status,
                Notes = s.Notes,
            }).ToList();

            return entity;
        }

        public async Task<HousekeepingDtoUpdate> GetHouseKeepingById(int Id, CancellationToken cancellationToken = default)
        {
            var house = await _context.Housekeeping
                .FirstOrDefaultAsync(x => x.HousekeepingId == Id, cancellationToken);

            if (house == null)
            {
                Console.WriteLine($"The record with ID {Id} was not found.");
                return null!;
            }

            var entity = house.Adapt<HousekeepingDtoUpdate>();
            return entity;
        }


        public async Task<bool> UpdateHouseKeeping(HousekeepingDtoUpdate dto, CancellationToken cancellationToken = default)
        {
            var entity = dto.Adapt<Housekeeping>();
            _context.Housekeeping.Update(entity);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
