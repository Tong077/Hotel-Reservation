namespace H_Domain.Models
{
    public class Employee
    {

        public int EmployeeId { get; set; }


        public string? FullName { get; set; }


        public string? Email { get; set; }


        public string? Phone { get; set; }


        public string? Role { get; set; }


        public string? ShiftTime { get; set; }

        public DateTime? HireDate { get; set; }


        public string? Status { get; set; }

        public ICollection<Housekeeping>? HousekeepingTasks { get; set; }
    }
}
