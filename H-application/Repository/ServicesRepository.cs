using H_application.DTOs.ServicesDto;
using H_application.Service;
using H_Domain.DataContext;
using H_Domain.Models;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace H_application.Repository
{
    public class ServicesRepository : IServicesService
    {
        private readonly EntityContext _context;

        public ServicesRepository(EntityContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateService(ServicesDtoCreate dto, CancellationToken cancellationToken = default)
        {
            var entity = dto.Adapt<Services>();
            await _context.Services.AddAsync(entity, cancellationToken);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteService(ServicesDtoUpdate dto, CancellationToken cancellationToken = default)
        {
            var entity = dto.Adapt<Services>();
            _context.Services.Remove(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<List<ServicesResponse>> GetallService(CancellationToken cancellationToken = default)
        {
            var service = await _context.Services
                 .OrderBy(s => s.ServiceName)
                 .AsNoTracking()
                 .ToListAsync();
            var entity = service.Adapt<List<ServicesResponse>>();
            return entity;
        }

        public async Task<ServicesDtoUpdate> GetServiceById(int Id, CancellationToken cancellationToken = default)
        {
            var service = await _context.Services.FirstOrDefaultAsync(s => s.ServiceId == Id, cancellationToken);
            var entity = service.Adapt<ServicesDtoUpdate>();
            return entity;
        }

        public async Task<bool> UpdateService(ServicesDtoUpdate dto, CancellationToken cancellation = default)
        {
            var entity = dto.Adapt<Services>();
            _context.Services.Update(entity);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
