using H_application.DTOs.RoomDto;
using H_Domain.Models;

namespace H_application.DTOs.ReservationDto
{
    public class ReservationResponse
    {
        public int ReservationId { get; set; }
        public int? GuestId { get; set; }
        public int? RoomId { get; set; }
        public DateTime? CheckInDate { get; set; }
        public DateTime? CheckOutDate { get; set; }
        public decimal? TotalPrice { get; set; }
        public decimal? RoomPrice { get; set; }
        public string? RoomCurrency { get; set; }
        public string? Currency { get; set; }
        public string? Status { get; set; }
        public string? RoomNumber { get; set; }
        public string? RoomTypeName { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public int? CurrentMonthTotal { get; set; }
        public decimal? GrowthPercentage { get; set; }

        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;

        public Guest? guest { get; set; }
        public List<RoomResponse> Rooms { get; set; } = new List<RoomResponse>();

        public DateTime? Date { get; set; }
        public string? GuestName { get; set; }
    }
}
