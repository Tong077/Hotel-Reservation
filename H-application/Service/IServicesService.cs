using H_application.DTOs.ServicesDto;

namespace H_application.Service
{
    public interface IServicesService
    {
        Task<bool> CreateService(ServicesDtoCreate dto, CancellationToken cancellationToken = default);
        Task<bool> DeleteService(ServicesDtoUpdate dto, CancellationToken cancellationToken = default);
        Task<bool> UpdateService(ServicesDtoUpdate dto, CancellationToken cancellation = default);
        Task<ServicesDtoUpdate> GetServiceById(int Id, CancellationToken cancellationToken = default);
        Task<List<ServicesResponse>> GetallService(CancellationToken cancellationToken = default);
    }
}
