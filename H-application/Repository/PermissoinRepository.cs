using H_Application.DTOs.PermissionDto;
using H_Application.Service;
using H_Domain.DataContext;

using H_Domain.Models;
using Mapster;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H_Application.Repository
{
    public class PermissoinRepository : IPermissionService
    {
        private readonly EntityContext _context;
        public PermissoinRepository(EntityContext context)
        {
            _context = context;
        }

        public async Task<bool> CraetePermission(PermissionDtoCreate permission, CancellationToken cancellation = default)
        {
            var permisson = permission.Adapt<Permission>();
            await _context.permissions.AddAsync(permisson);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeletePermisson(PermissionDtoUpdate permissionDto, CancellationToken cancellation = default)
        {
            var permissson = permissionDto.Adapt<Permission>();
            _context.permissions.Remove(permissson);
            return await _context.SaveChangesAsync(cancellation) > 0;
        }

        public async Task<List<PermissionRespone>> GetallPermission(CancellationToken cancellation = default)
        {
            var permisson = await _context.permissions
                .OrderBy(x => x.Name)
                .AsNoTracking().ToListAsync();
            return permisson.Select(p => p.Adapt<PermissionRespone>()).ToList();
        }

        public async Task<PermissionDtoUpdate> GetById(int id, CancellationToken cancellationToken = default)
        {
            var permission = await _context.permissions
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
            var per = permission.Adapt<PermissionDtoUpdate>();
            return per;
        }

        public async Task<bool> UpdatePermission(PermissionDtoUpdate permission, CancellationToken cancellation = default)
        {
            var per = permission.Adapt<Permission>();
            _context.permissions.Update(per);
            return await _context.SaveChangesAsync() > 0;

        }
    }
}
