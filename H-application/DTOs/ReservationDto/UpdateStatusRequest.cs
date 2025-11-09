using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H_application.DTOs.ReservationDto
{
    public class UpdateStatusRequest
    {
        public string? RoomNumber { get; set; } 
        public DateTime? Date { get; set; }
        public string? NewStatus { get; set; }
    }
}
