using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H_application.DTOs.UserDto
{
    public class UserDtoCreate
    {
        public IFormFile? Image { get; set; }
        public string Username { get; set; } = null!;
        
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string? PhoneNumber { get; set; }
    }
}
