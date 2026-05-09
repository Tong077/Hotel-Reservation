using System.ComponentModel.DataAnnotations.Schema;

namespace H_Domain.Models
{
    public class Review
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ReviewId { get; set; }


        public int? GuestId { get; set; }


        public int? ReservationId { get; set; }


        public int? Rating { get; set; }


        public string? Comment { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public Guest? Guest { get; set; }
        public Reservation? Reservation { get; set; }
    }
}
