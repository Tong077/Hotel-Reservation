using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H_application.DTOs.ReservationDto
{
    public class SwitchRoomRequest
    {
        public int ReservationId { get; set; }
        public string OldRoomNumber { get; set; } = "";
        public string NewRoomNumber { get; set; } = "";
        public DateTime Date { get; set; }
       
    }
}