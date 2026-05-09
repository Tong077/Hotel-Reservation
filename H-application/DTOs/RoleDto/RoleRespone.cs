using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H_Application.DTOs.RoleDto
{
    public class RoleRespone
    {
        

        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }

        public List<PermissionItemDto> Permissions { get; set; } = new();

    }
}
