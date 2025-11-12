using H_application.DTOs.HotelDto;
using H_application.Service;
using H_Domain.DataContext;
using H_Domain.Models;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace H_Reservation.Service
{
    public class HotelRepository : IHotelServicecs
    {
        private readonly EntityContext? _context;
        public HotelRepository(EntityContext? context)
        {
            _context = context;
        }
        public async Task<bool> CreatehotelAsync(HotelDtoCreate hotelDotCreate, CancellationToken cancellationToken)
        {
            var entity = hotelDotCreate.Adapt<Hotels>();
            await _context!.Hotels.AddAsync(entity, cancellationToken);
            return await _context.SaveChangesAsync() > 0;

        }

        public async Task<bool> DeletehotelAsync(HotelDtoUpdate hotelDtoUpdate, CancellationToken cancellation)
        {
            var entity = hotelDtoUpdate.Adapt<Hotels>();
            _context!.Hotels.Remove(entity);
            return await _context.SaveChangesAsync() > 0;

        }

        public async Task<IEnumerable<HotelResponse>> GetAllHotelAsync(CancellationToken cancellationToken)
        {
            var result = await _context!.Hotels
               .OrderBy(h => h.Name)
               .AsNoTracking().ToListAsync();
            var hotel = result.Adapt<List<HotelResponse>>();
            return hotel;
        }

        public async Task<HotelDtoUpdate> GetHotelByIdAsync(int Id, CancellationToken cancellationToken)
        {
            var hotel = await _context!.Hotels
                .FirstOrDefaultAsync(h => h.HotelId == Id, cancellationToken);
            var hotelResponse = hotel.Adapt<HotelDtoUpdate>();
            return hotelResponse!;
        }

        public async Task<bool> UpdatehotelAsync(HotelDtoUpdate hotelDtoUpdate, CancellationToken cancellationToken)
        {
            var entity = hotelDtoUpdate.Adapt<Hotels>();
            _context!.Hotels.Update(entity);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
