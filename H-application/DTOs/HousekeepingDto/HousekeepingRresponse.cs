namespace H_application.DTOs.HousekeepingDto
{
    public class HousekeepingRresponse
    {
        public int HousekeepingId { get; set; }

        public int? RoomId { get; set; }


        public int? EmployeeId { get; set; }


        public string? Status { get; set; }

        public DateTime? LastCleanedDate { get; set; }


        public string? Notes { get; set; }

        public string? RoomNumber { get; set; }
        public string? RoomTypeName { get; set; }
        public string? EmployeeName { get; set; }
    }
}
