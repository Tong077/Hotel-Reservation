using H_application.DTOs.ReservationDto;
using H_Domain.Models;
using System.Threading.Tasks;

namespace H_application.Service
{
    public interface IReservationService
    {
       Task<bool> CreateReservationAsync(ReservationDtoCreate reservation,CancellationToken cancellationToken);
        Task<bool> UpdateReservationAsync(ReservationDtoUpdate reservation,CancellationToken cancellationToken);
        Task<bool> DeleteReservationAsync(ReservationDtoUpdate reservation,CancellationToken cancellationToken);

        Task<ReservationDtoUpdate> GetReservationByIdAsync(int Id,CancellationToken cancellationToken);
        Task<IEnumerable<ReservationResponse>> GetAllReservationAsync(string filter ,CancellationToken cancellationToken);

        Task<bool> UpdateReservationStatusAsync(ReservationDtoUpdate dto,CancellationToken cancellation);
        Task<ReservationResponse> TotalReservation(CancellationToken cancellationToken);
        Task<List<MonthlyRevenueDto>> GetMonthlyRevenueByCurrencyAsync(string currency, int year, CancellationToken cancellationToken);
        Task<ReservationResponse> GetCheckInTrendAsync(CancellationToken cancellationToken);
        Task<ReservationResponse> ConfirmReservationAsync(CancellationToken cancellationToken);
        Task<ReservationResponse> PendingReservatoin(CancellationToken cancellationToken);

        Task<List<RoomCalendarDto>> GetRoomCalendarAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken);
    }
}
