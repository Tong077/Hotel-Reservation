using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H_application.DTOs.ReservationServicesDto
{
    public class ReservationServicesResponse
    {
        public int ReservationServiceId { get; set; }


        public int? ReservationId { get; set; }


        public int? ServiceId { get; set; }

        public decimal? Quantity { get; set; }


        public decimal? TotalPrice { get; set; }



        public int GuestId { get; set; }
        public int RoomId { get; set; }
        public int RoomTypeId { get; set; } 
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? RoomNumber { get; set; }
        public string? RoomTypeName { get; set; }
        public string? ServiceName { get; set; }
        public decimal? ServicePrice { get; set; }
        public DateTime? CheckIn { get; set; }
        public DateTime? CheckOut { get; set; }
        public decimal? BasePrice { get; set; }
    }
}
