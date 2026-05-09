using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace H_Domain.Models
{
    public class Hotels
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int HotelId { get; set; }


        public string? Name { get; set; }


        public string? Address { get; set; }


        public string? City { get; set; }

        public string? Email { get; set; }
        public string? Phone { get; set; }
        public ICollection<Room>? Rooms { get; set; }



    }
}
