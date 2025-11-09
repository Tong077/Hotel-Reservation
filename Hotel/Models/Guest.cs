using System.ComponentModel.DataAnnotations;

namespace H_Domain.Models   // ✅ corrected
{
    public class Guest
    {
        [Key]
        public int GuestId { get; set; }

        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        [EmailAddress]
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
