using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H_Domain.Models
{
    public class Review
    {
       
        public int ReviewId { get; set; }

        
        public int? GuestId { get; set; }

        
        public int? ReservationId { get; set; }

        
        public int? Rating { get; set; }

       
        public string? Comment { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public Guest? Guest { get; set; }
        public Reservation? Reservation { get; set; }
    }
}
