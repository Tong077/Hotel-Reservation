using System.ComponentModel.DataAnnotations;

namespace H_Domain.Models
{
    public class Reservation
    {
        [Key]
        public int ReservationId { get; set; }


        public int? GuestId { get; set; }


        public int? RoomId { get; set; }

        public DateTime? CheckInDate { get; set; }

        public string? Currency { get; set; }
        public DateTime? CheckOutDate { get; set; }


        public decimal? TotalPrice { get; set; }


        public string? Status { get; set; }

        public DateTime? CreatedAt { get; set; }

        public Guest? guest { get; set; }
        public Room? rooms { get; set; }


        public int? PaymentId { get; set; }

        public Payment? Payment { get; set; }
        public ICollection<ReservationService>? ReservationServices { get; set; }

    }
}
