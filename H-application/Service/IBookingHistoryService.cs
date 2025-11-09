using H_application.DTOs.BookingHistoryDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H_application.Service
{
    public interface IBookingHistoryService
    {
        Task<bool> CreateBookingHistory(BookingHistoryDtoCreate dto, CancellationToken cancellationToken = default);
        Task<bool> UpdateBookingHistory(BookingHistoryDtoUpdate dto, CancellationToken cancellationToken = default);
        Task<bool> DeleteBookingHistory(BookingHistoryDtoUpdate dto, CancellationToken cancellation = default);
        Task<BookingHistoryDtoUpdate> GetBookingHistoryById(int Id,  CancellationToken cancellationToken = default);  
        Task<List<BookingHistoryResponse>> GetBookingHistoryList(CancellationToken cancellationToken = default);
    }
}
