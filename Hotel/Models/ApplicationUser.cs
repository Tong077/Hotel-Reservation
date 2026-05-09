using Microsoft.AspNetCore.Identity;

namespace H_Domain.Models
{
    public class ApplicationUser : IdentityUser<int>
    {
        public string? ImageUrl { get; set; }
    }
}