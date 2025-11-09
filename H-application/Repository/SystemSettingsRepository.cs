using Dapper;
using H_application.DTOs.SystemSettingsDto;
using H_application.Service;
using H_Domain.DataContext;
using H_Domain.Models;
using Mapster;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H_application.Repository
{
    public class SystemSettingsRepository : ISystemSettingsService
    {
        private readonly EntityContext _context;

        public SystemSettingsRepository(EntityContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateSetting(SystemSettingsDtoCreate dto, CancellationToken cancellation = default)
        {
            var exists = await _context.SystemSettings
                .AnyAsync(s => s.Key == dto.Key, cancellation);
            if (exists)
            {
                Console.WriteLine("Key already exists: " + dto.Key);
                return false;
            }


            var entity = new SystemSetting
            {
                Key = dto.Key,
                Value = dto.Value,
                Category = dto.Category,
                Description = dto.Description,
                IsActive = dto.IsActive ? true : false,
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow,
            };
            _context.SystemSettings.Add(entity);
            var saveResult = await _context.SaveChangesAsync();
            return saveResult > 0;
        }

        public async Task<bool> DeleteSetting(SystemSettingsDtoUpdate dto, CancellationToken cancellation = default)
        {
            var entity = dto.Adapt<SystemSetting>();

            _context.SystemSettings.Remove(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<List<SystemSettingsResponse>> GetAllSystemSetting(CancellationToken cancellation = default)
        {
            return await _context.SystemSettings
               .Select(s => new SystemSettingsResponse
               {
                   SettingId = s.SettingId,
                   Key = s.Key,
                   Value = s.Value,
                   Category= s.Category,
                   Description = s.Description,
                   IsActive = s.IsActive,
               }).ToListAsync(cancellation);
        }

        public async Task<SystemSettingsResponse?> GetSettingByKeyAsync(string key, CancellationToken cancellationToken)
        {
            var setting = await _context.SystemSettings
                .FirstOrDefaultAsync(s => s.Key == key && s.IsActive == true, cancellationToken);

            if (setting == null)
                return null;

            return new SystemSettingsResponse
            {
                SettingId = setting.SettingId,
                Key = setting.Key,
                Value = setting.Value,
                Category = setting.Category,
                Description = setting.Description,
                IsActive = setting.IsActive,
            };
        }

        public async Task<SystemSettingsDtoUpdate> GetSystemSettingById(int Id, CancellationToken cancellation = default)
        {
            var setting = await _context.SystemSettings.FindAsync(new object[] { Id }, cancellation);
            if (setting == null)
                return null!;

            return new SystemSettingsDtoUpdate
            {
                SettingId = setting.SettingId,
                Key = setting.Key,
                Value = setting.Value,
                Category = setting.Category,
                Description = setting.Description,
                IsActive = setting.IsActive ?? true,
                CreatedDate = setting.CreatedDate,
                UpdatedDate = setting.UpdatedDate,
            };
            
           
        }

        public async Task<string?> GetValueAsync(string key, CancellationToken cancellationToken)
        {
            var setting = await _context.SystemSettings
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Key == key && s.IsActive == true,cancellationToken);

            return setting!.Value;
        }

        public async Task<bool> UpdateSetting(SystemSettingsDtoUpdate dto, CancellationToken cancellation = default)
        {
            var entity = dto.Adapt<SystemSetting>();

            _context.SystemSettings.Update(entity);
            return await _context.SaveChangesAsync(cancellation) > 0;
        }
    }
}
