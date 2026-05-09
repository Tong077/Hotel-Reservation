using System.ComponentModel.DataAnnotations.Schema;

namespace H_Domain.Models
{
    public class ReservationService
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ReservationServiceId { get; set; }


        public int? ReservationId { get; set; }


        public int? ServiceId { get; set; }

        public decimal? Quantity { get; set; }


        public decimal? TotalPrice { get; set; }

        public Reservation? Reservation { get; set; }
        public Services? Service { get; set; }
    }
}
