using H_Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H_application.DTOs.RoomTypeDto
{
    public class RoomTypeResponse
    {
        public int RoomTypeId { get; set; }


        public string? Name { get; set; }


        public string? Description { get; set; }


        public decimal? PricePerNight { get; set; }

        public string? Currency { get; set; }
        public decimal? Capacity { get; set; }
        public ICollection<Room>? Rooms { get; set; }
    }
}
