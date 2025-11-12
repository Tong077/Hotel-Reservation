namespace H_application.DTOs.PaymentDto
{
    public class ReservationForPaymentDto
    {
        public int ReservationId { get; set; }
        public string? GuestFullName { get; set; }
        public string? RoomNumber { get; set; }
        public decimal? TotalPrice { get; set; }
        public string? DisplayTotal { get; set; }
        public string? Currency { get; set; }
    }

}
