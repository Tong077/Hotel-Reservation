using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H_Domain.Models
{
    public class ApplicationRole : IdentityRole<int>
    {

        public bool? IsActive { get; set; }

        public ICollection<RolePermission>? RolePermissions { get; set; }
    }
}
