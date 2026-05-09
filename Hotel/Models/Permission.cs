using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H_Domain.Models
{
    public class Permission
    {
        public int? Id { get; set; }
        public string? Name { get; set; }   
        public DateTime Created_at { get; set; } = DateTime.Now;   
        public DateTime Updated_at { get; set; } = DateTime.Now;
        public ICollection<RolePermission>? RolePermissions { get; set; }
    }
}
