using H_Application.DTOs.RoleDto;
using H_Application.Service;
using H_Domain.DataContext;
using H_Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H_Application.Repository
{
    public class RoleRepository : IRoleService
    {
        private readonly EntityContext _context;

        public RoleRepository(EntityContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateRole(RoleCreateDto dto, CancellationToken cancellation = default)
        {
            var role = new ApplicationRole
            {
                Name = dto.Name,
                NormalizedName = dto.Name.ToUpper(),
                IsActive = dto.IsActive,
                RolePermissions = dto.PermissionIds.Select(pid => new RolePermission
                {
                    PermissionId = pid,
                }).ToList()
            };

            await _context.Roles.AddAsync(role, cancellation); 
            return await _context.SaveChangesAsync(cancellation) > 0;
        }

        public async Task<bool> DeleteRole(RoleUpdateDto roleUpdateDto, CancellationToken cancellation = default)
        {
            var role = await _context.Roles
                .Include(r => r.RolePermissions)
                .FirstOrDefaultAsync(r => r.Name == roleUpdateDto.Name, cancellation);

            if (role == null)
                return false;
            _context.RolePermissions.RemoveRange(role.RolePermissions);

            _context.Roles.Remove(role);

            return await _context.SaveChangesAsync(cancellation) > 0;
        }

        public async Task<List<RoleRespone>> GetAllRoles(CancellationToken cancellation = default)
        {
            var roles = await _context.Roles
            .Include(r => r.RolePermissions)
                .ThenInclude(rp => rp.Permission)
            .AsNoTracking()
            .ToListAsync(cancellation);

            return roles.Select(role => new RoleRespone
            {
                Id = role.Id,
                Name = role.Name,
                IsActive = role.IsActive ?? false,
                Permissions = role.RolePermissions
                    .Select(rp => new PermissionItemDto
                    {
                        Id = rp.Permission.Id ?? 0,
                        Name = rp.Permission.Name
                    }).ToList()
            }).ToList();
        }
        

        public async Task<RoleRespone?> GetRoleById(int id, CancellationToken cancellation = default)
        {
            var role = await _context.Roles
         .Include(r => r.RolePermissions)
             .ThenInclude(rp => rp.Permission)
         .FirstOrDefaultAsync(r => r.Id == id, cancellation);

            if (role == null) return null;

            return new RoleRespone
            {
                Id = role.Id,
                Name = role.Name,
                IsActive = role.IsActive ?? false,
                Permissions = role.RolePermissions
                    .Select(rp => new PermissionItemDto
                    {
                        Id = rp.Permission.Id ?? 0,
                        Name = rp.Permission.Name
                    }).ToList()
            };
        }

        public async Task<bool> UpdateRole(RoleUpdateDto dto, CancellationToken cancellation = default)
        {
            var role = await _context.Roles
             .Include(r => r.RolePermissions)
             .FirstOrDefaultAsync(r => r.Id == dto.Id, cancellation);

            if (role == null) return false;

            // update basic info
            role.Name = dto.Name;
            role.NormalizedName = dto.Name.ToUpper();
            role.IsActive = dto.IsActive;

            // 🔥 remove old permissions
            _context.RemoveRange(role.RolePermissions);

            // 🔥 add new permissions
            role.RolePermissions = dto.PermissionIds.Select(pid => new RolePermission
            {
                RoleId = role.Id,
                PermissionId = pid
            }).ToList();

            return await _context.SaveChangesAsync(cancellation) > 0;
        }
    }
}
