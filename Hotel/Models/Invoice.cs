namespace H_Domain.Models
{
    public class Invoice
    {

        public int InvoiceId { get; set; }


        public int? ReservationId { get; set; }
        public int? PaymentId { get; set; }
        public decimal? TotalAmount { get; set; }
        public decimal? TaxAmount { get; set; }
        public decimal? GrandTotal { get; set; }

        public DateTime? IssuedDate { get; set; }


        public string? FilePath { get; set; }

        public Reservation? Reservation { get; set; }
        public Payment? Payment { get; set; }
    }
}
