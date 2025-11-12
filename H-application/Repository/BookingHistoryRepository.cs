using H_application.DTOs.BookingHistoryDto;
using H_application.Service;
using H_Domain.DataContext;
using H_Domain.Models;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace H_application.Repository
{
    public class BookingHistoryRepository : IBookingHistoryService
    {
        private readonly EntityContext _context;

        public BookingHistoryRepository(EntityContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateBookingHistory(BookingHistoryDtoCreate dto, CancellationToken cancellationToken = default)
        {
            var entity = dto.Adapt<BookingHistory>();
            await _context.BookingHistory.AddAsync(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteBookingHistory(BookingHistoryDtoUpdate dto, CancellationToken cancellation = default)
        {
            var entity = dto.Adapt<BookingHistory>();
            _context.BookingHistory.Remove(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<BookingHistoryDtoUpdate> GetBookingHistoryById(int Id, CancellationToken cancellationToken = default)
        {
            var booking = await _context.BookingHistory
                .FirstOrDefaultAsync(b => b.HistoryId == Id, cancellationToken);
            var entity = booking.Adapt<BookingHistoryDtoUpdate>();
            return entity;
        }

        public async Task<List<BookingHistoryResponse>> GetBookingHistoryList(CancellationToken cancellationToken = default)
        {
            var booking = await _context.BookingHistory
                .Include(b => b.Reservation)
                .AsNoTracking()
                .ToListAsync();
            var entity = booking.Adapt<List<BookingHistoryResponse>>();
            return entity;
        }

        public async Task<bool> UpdateBookingHistory(BookingHistoryDtoUpdate dto, CancellationToken cancellationToken = default)
        {
            var entity = dto.Adapt<BookingHistory>();
            _context.BookingHistory.Update(entity);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
