namespace H_application.DTOs.RoomDto
{
    public class RoomResponse
    {
        public int RoomId { get; set; }


        public string? RoomNumber { get; set; }


        public int? RoomTypeId { get; set; }
        public string? RoomTypeName { get; set; }

        public decimal? RoomPrice { get; set; }
        public string? Currency { get; set; }
        public string? Status { get; set; }
        public string? Images { get; set; }
        public string RoomCurrency { get; set; }
        public int? HotelId { get; set; }
        public string? HotelName { get; set; }
        public List<int>? SelectedRoomIds { get; set; } = new();

    }
}
