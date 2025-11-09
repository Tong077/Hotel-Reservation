using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H_application.DTOs.HousekeepingDto
{
    public class HousekeepingDtoUpdate
    {
        public int HousekeepingId { get; set; }


        public int? RoomId { get; set; }


        public int? EmployeeId { get; set; }


        public string? Status { get; set; }

        public DateTime? LastCleanedDate { get; set; }


        public string? Notes { get; set; }

    }
}
