namespace H_application.DTOs.InvoicesDto
{
    public class InvoicesDtoUpdate
    {
        public int InvoiceId { get; set; }


        public int? PaymentId { get; set; }
        public int? PaymentMethodId { get; set; }

        public decimal? TotalAmount { get; set; }
        public decimal? TaxAmount { get; set; }
        public decimal? GrandTotal { get; set; }

        public DateTime? IssuedDate { get; set; } = DateTime.Now;


        public string? FilePath { get; set; }
    }
}
