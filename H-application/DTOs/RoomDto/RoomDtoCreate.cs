using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H_application.DTOs.RoomDto
{
    public class RoomDtoCreate
    {
        public string? RoomNumber { get; set; }
        public int? RoomTypeId { get; set; }

        public string? Images { get; set; }
        public string? Status { get; set; }

        public int? HotelId { get; set; }
    }
}
