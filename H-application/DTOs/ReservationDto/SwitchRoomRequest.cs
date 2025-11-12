namespace H_application.DTOs.ReservationDto
{
    public class SwitchRoomRequest
    {
        public int ReservationId { get; set; }
        public string OldRoomNumber { get; set; } = "";
        public string NewRoomNumber { get; set; } = "";
        public DateTime Date { get; set; }

        public string NewStatus { get; set; } = "";

    }
}