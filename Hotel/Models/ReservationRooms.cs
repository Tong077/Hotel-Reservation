using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H_Domain.Models
{
    public class ReservationRooms
    {
        public int Id { get; set; }
        public int? RoomId { get; set; }    
        public int? ReservatinId { get; set; }  
    }
}
