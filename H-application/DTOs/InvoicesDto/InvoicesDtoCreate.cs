namespace H_application.DTOs.InvoicesDto
{
    public class InvoicesDtoCreate
    {

        public int InvoiceId { get; set; }
        public int? ReservationId { get; set; }


        public int? PaymentId { get; set; }

        public decimal? TotalAmount { get; set; }
        public decimal? TaxAmount { get; set; }
        public decimal? GrandTotal { get; set; }

        public DateTime? IssuedDate { get; set; } = DateTime.Now;


        public string? FilePath { get; set; }
    }
}
