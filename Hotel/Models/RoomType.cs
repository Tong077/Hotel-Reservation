using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace H_Domain.Models
{
    public class RoomType
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RoomTypeId { get; set; }


        public string? Name { get; set; }


        public string? Description { get; set; }


        public decimal? PricePerNight { get; set; }

        public string? Currency { get; set; }
        public decimal? Capacity { get; set; }

        public ICollection<Room>? Rooms { get; set; }


    }
}
