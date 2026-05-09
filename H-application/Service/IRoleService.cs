using H_Application.DTOs.RoleDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H_Application.Service
{
    public interface IRoleService
    {
        Task<bool> CreateRole(RoleCreateDto dto, CancellationToken cancellation = default);

        Task<bool> UpdateRole(RoleUpdateDto dto, CancellationToken cancellation = default);

        Task<bool> DeleteRole(RoleUpdateDto roleUpdateDto, CancellationToken cancellation = default);

        Task<RoleRespone?> GetRoleById(int id, CancellationToken cancellation = default);

        Task<List<RoleRespone>> GetAllRoles(CancellationToken cancellation = default);
    }
}
