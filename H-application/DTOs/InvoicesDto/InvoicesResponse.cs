namespace H_application.DTOs.InvoicesDto
{
    public class InvoicesResponse
    {
        public int InvoiceId { get; set; }


        public int? ReservationId { get; set; }
        public int? PaymentId { get; set; }

        public decimal? TotalAmount { get; set; }
        public decimal? TaxAmount { get; set; }
        public decimal? GrandTotal { get; set; }

        public DateTime? IssuedDate { get; set; }


        public string? FilePath { get; set; }
        public string? GuestName { get; set; }
        public string? RoomNumber { get; set; }
        public string? RoomType { get; set; }
        public string? PaymentMethod { get; set; }
        public string? HotelName { get; set; }
        public string? StreetName { get; set; }
        public string? Address { get; set; }
        public DateTime? CheckInDate { get; set; }
        public DateTime? CheckOutDate { get; set; }
        public List<InvoiceServiceItem>? Services { get; set; }
    }
    public class InvoiceServiceItem
    {
        public string? ServiceName { get; set; }
        public int? Quantity { get; set; }
        public decimal? Price { get; set; }
        public decimal Total => Price * Quantity ?? 0;
    }
}
