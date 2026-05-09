using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H_Application.DTOs.RoleDto
{
    public class RoleCreateDto
    {
        public string Name { get; set; }
        public bool IsActive { get; set; }

      
        public List<int> PermissionIds { get; set; } = new();
    }
}
