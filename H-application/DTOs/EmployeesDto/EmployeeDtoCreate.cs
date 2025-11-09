using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H_application.DTOs.EmployeesDto
{
    public class EmployeeDtoCreate
    {

        public string? FullName { get; set; }


        public string? Email { get; set; }


        public string? Phone { get; set; }


        public string? Role { get; set; }


        public string? ShiftTime { get; set; }

        public DateTime? HireDate { get; set; }


        public string? Status { get; set; }
    }
}
