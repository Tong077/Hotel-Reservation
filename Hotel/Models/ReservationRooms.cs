using System.ComponentModel.DataAnnotations.Schema;

namespace H_Domain.Models
{
    public class ReservationRooms
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int? RoomId { get; set; }
        public int? ReservatinId { get; set; }
    }
}
