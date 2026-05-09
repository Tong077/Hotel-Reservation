using System.ComponentModel.DataAnnotations.Schema;

namespace H_Domain.Models
{
    public class Branch
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BranchId { get; set; }


        public string? BranchName { get; set; }


        public string? Address { get; set; }


        public string? City { get; set; }


        public string? ContactNumber { get; set; }


        public string? Email { get; set; }

        public ICollection<Room>? Rooms { get; set; }
    }
}
