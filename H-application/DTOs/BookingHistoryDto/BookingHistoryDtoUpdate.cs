namespace H_application.DTOs.BookingHistoryDto
{
    public class BookingHistoryDtoUpdate
    {
        public int HistoryId { get; set; }


        public int? ReservationId { get; set; }


        public int? GuestId { get; set; }

        public int? RoomId { get; set; }

        public DateTime? CheckInDate { get; set; }
        public DateTime? CheckOutDate { get; set; }

        public decimal? TotalAmount { get; set; }


        public string? Status { get; set; }
    }
}
