using H_application.DTOs.ReservationServicesDto;

namespace H_application.Service
{
    public interface IReservationServicesService
    {
        Task<bool> CreateReservationService(ReservationServicesDtoCreate dto, CancellationToken cancellationToken = default);
        Task<bool> UpdateReservationService(ReservationServicesDtoUpdate dto, CancellationToken cancellationToken = default);
        Task<bool> DeleteReservationService(ReservationServicesDtoUpdate dto, CancellationToken cancellationToken = default);
        Task<ReservationServicesDtoUpdate> GetReservationById(int Id, CancellationToken cancellationToken = default);
        Task<IEnumerable<ReservationServicesResponse>> GetAllReservationService(CancellationToken cancellationToken = default);
    }
}
