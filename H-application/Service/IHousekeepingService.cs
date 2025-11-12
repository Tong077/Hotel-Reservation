using H_application.DTOs.HousekeepingDto;

namespace H_application.Service
{
    public interface IHousekeepingService
    {
        Task<bool> CreateHouseKeeping(HousekeepingDtoCreate dto, CancellationToken cancellationToken = default);
        Task<bool> UpdateHouseKeeping(HousekeepingDtoUpdate dto, CancellationToken cancellationToken = default);
        Task<bool> DeleteHouseKeeping(HousekeepingDtoUpdate dto, CancellationToken cancellationToken = default);
        Task<HousekeepingDtoUpdate> GetHouseKeepingById(int Id, CancellationToken cancellationToken = default);
        Task<List<HousekeepingRresponse>> GetHouseKeepingAll(CancellationToken cancellationToken = default);
    }
}
