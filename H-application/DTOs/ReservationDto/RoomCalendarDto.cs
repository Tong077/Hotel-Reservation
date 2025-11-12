namespace H_application.DTOs.ReservationDto
{
    public class RoomCalendarDto
    {
        public int ReservationId { get; set; }
        public DateTime Date { get; set; }
        public string? RoomNumber { get; set; }
        public string? RoomType { get; set; }
        public string? GuestName { get; set; }
        public string? Status { get; set; }

        public decimal? TotalPrice { get; set; }
        public DateTime? CheckInDate { get; set; }
        public DateTime? CheckOutDate { get; set; }
    }
}
