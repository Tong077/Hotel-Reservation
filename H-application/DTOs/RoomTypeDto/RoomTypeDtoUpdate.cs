namespace H_application.DTOs.RoomTypeDto
{
    public class RoomTypeDtoUpdate
    {
        public int RoomTypeId { get; set; }


        public string? Name { get; set; }


        public string? Description { get; set; }


        public decimal? PricePerNight { get; set; }

        public string? Currency { get; set; }
        public decimal? Capacity { get; set; }
    }
}
