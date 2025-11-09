using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H_Domain.Models
{
    public class Branch
    {
       
        public int BranchId { get; set; }

        
        public string? BranchName { get; set; }

       
        public string? Address { get; set; }

       
        public string? City { get; set; }

        
        public string? ContactNumber { get; set; }

        
        public string? Email { get; set; }

        public ICollection<Room>? Rooms { get; set; }
    }
}
