using H_application.DTOs.SystemSettingsDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H_application.Service
{
    public interface ISystemSettingsService
    {
        Task<bool> CreateSetting(SystemSettingsDtoCreate dto, CancellationToken cancellation = default);
        Task<bool> UpdateSetting(SystemSettingsDtoUpdate dto, CancellationToken cancellation = default);

        Task<bool> DeleteSetting(SystemSettingsDtoUpdate dto,CancellationToken cancellation = default);
        Task<SystemSettingsDtoUpdate> GetSystemSettingById(int Id,CancellationToken cancellation = default);
        Task<List<SystemSettingsResponse>> GetAllSystemSetting(CancellationToken cancellation = default);
        Task<string?> GetValueAsync(string key, CancellationToken cancellationToken);
        Task<SystemSettingsResponse?> GetSettingByKeyAsync(string key, CancellationToken cancellationToken);
    };
};


