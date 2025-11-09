using H_application.DTOs.RoomDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H_application.DTOs.ReservationDto
{
    public class ReservationDtoCreate
    {
       


        public int? GuestId { get; set; }


        public List<int>? RoomId { get; set; } = new ();
       
        public DateTime? CheckInDate { get; set; }

        public DateTime? CheckOutDate { get; set; }

        public string? Currency { get; set; }
        public decimal? TotalPrice { get; set; }

        public string? Status { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public List<RoomResponse> Rooms { get; set; } = new();
    }
}
