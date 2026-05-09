using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace H_Domain.Models
{
    public class Room
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RoomId { get; set; }


        public string? RoomNumber { get; set; }


        public int? RoomTypeId { get; set; }

        public string? Images { get; set; }
        public string? Status { get; set; }

        public int? HotelId { get; set; }

        public ICollection<Reservation>? Reservations { get; set; }
        public RoomType? roomType { get; set; }
        public Hotels? hotel { get; set; }
        public ICollection<Housekeeping>? Housekeepings { get; set; }
    }

}
