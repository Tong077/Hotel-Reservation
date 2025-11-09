using H_Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H_application.DTOs.HotelDto
{
    public class HotelResponse
    {
        public int HotelId { get; set; }


        public string? Name { get; set; }


        public string? Address { get; set; }


        public string? City { get; set; }

        public string? Email { get; set; }
        public string? Phone { get; set; }
        //public ICollection<Room>? Rooms { get; set; }
    }
}
