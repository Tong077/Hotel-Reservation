using H_Application.DTOs.PermissionDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H_Application.Service
{
    public interface IPermissionService
    {
        Task<bool> CraetePermission(PermissionDtoCreate permission, CancellationToken cancellation = default);
        Task<List<PermissionRespone>> GetallPermission(CancellationToken cancellation = default);
        Task<PermissionDtoUpdate> GetById(int id,CancellationToken cancellationToken = default);
        Task<bool> UpdatePermission(PermissionDtoUpdate permission, CancellationToken cancellation = default);
        Task<bool> DeletePermisson(PermissionDtoUpdate permissionDto, CancellationToken cancellation = default);
    }
}
