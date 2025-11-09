using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H_Domain.Models
{
    public class Services
    {
       
        public int ServiceId { get; set; }

      
        public string? ServiceName { get; set; }

        
        public string? Category { get; set; }

       
        public decimal? Price { get; set; }

        public string? Currency { get; set; }
        public string? Description { get; set; }

        public bool? IsActive { get; set; }

        public ICollection<ReservationService>? ReservationServices { get; set; }
    }
}
