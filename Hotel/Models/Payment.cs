using System.ComponentModel.DataAnnotations;

namespace H_Domain.Models
{
    public class Payment
    {
        [Key]
        public int PaymentId { get; set; }


        public int? ReservationId { get; set; }

        //public int? CurrencyId { get; set; }
        public int? PaymentMethodId { get; set; }


        public decimal? Amount { get; set; }


        public string? Currency { get; set; }


        public string? TransactionId { get; set; }


        public string? PaymentStatus { get; set; }

        public DateTime? PaymentDate { get; set; } = DateTime.UtcNow;


        public decimal? RefundAmount { get; set; }

        public DateTime? RefundDate { get; set; }
        public PaymentMethod? PaymentMethod { get; set; }
        // Navigation properties
        //public Reservation? Reservation { get; set; }
        public ICollection<Reservation>? Reservation { get; set; }
        //public PaymentMethod PaymentMethod { get; set; }

    }

}
