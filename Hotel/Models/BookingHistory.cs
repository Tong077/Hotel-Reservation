using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H_Domain.Models
{
    public class BookingHistory
    {
       
        public int HistoryId { get; set; }

       
        public int? ReservationId { get; set; }

      
        public int? GuestId { get; set; }

        public int? RoomId { get; set; }

        public DateTime? CheckInDate { get; set; }
        public DateTime? CheckOutDate { get; set; }

        public decimal? TotalAmount { get; set; }

       
        public string? Status { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public Reservation? Reservation { get; set; }
        public Guest? Guest { get; set; }
    }
}
