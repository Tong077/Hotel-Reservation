using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H_Domain.Models
{
    public class ReservationService
    {
       
        public int ReservationServiceId { get; set; }

        
        public int? ReservationId { get; set; }

       
        public int? ServiceId { get; set; }

        public decimal? Quantity { get; set; }

        
        public decimal? TotalPrice { get; set; }

        public Reservation? Reservation { get; set; }
        public Services? Service { get; set; }
    }
}
