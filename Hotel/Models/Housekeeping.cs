namespace H_Domain.Models
{
    public class Housekeeping
    {

        public int HousekeepingId { get; set; }


        public int? RoomId { get; set; }


        public int EmployeeId { get; set; }


        public string? Status { get; set; }

        public DateTime? LastCleanedDate { get; set; } = DateTime.Now;


        public string? Notes { get; set; }

        public Room? Room { get; set; }
        public Employee? Employee { get; set; }
    }
}
