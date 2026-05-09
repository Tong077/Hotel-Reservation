using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H_application.DTOs.UserDto
{
    public class UserRespone
    {
        public string? Id { get; set; }
        public string? Image { get; set; }
        public string? Username { get; set; }
        public string? email { get; set; }
        public string? password { get; set; }
        public string? phonenumber { get; set; }

    }
}
