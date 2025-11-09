using H_application.DTOs.HousekeepingDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H_application.Service
{
    public interface IHousekeepingService
    {
        Task<bool> CreateHouseKeeping(HousekeepingDtoCreate dto, CancellationToken cancellationToken = default);
        Task<bool> UpdateHouseKeeping(HousekeepingDtoUpdate dto, CancellationToken cancellationToken = default);
        Task<bool> DeleteHouseKeeping(HousekeepingDtoUpdate dto,CancellationToken cancellationToken = default);
        Task<HousekeepingDtoUpdate> GetHouseKeepingById(int Id,CancellationToken cancellationToken = default);
        Task<List<HousekeepingRresponse>> GetHouseKeepingAll(CancellationToken cancellationToken = default);
    }
}
