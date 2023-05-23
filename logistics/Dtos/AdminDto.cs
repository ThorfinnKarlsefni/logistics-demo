using System;
using Microsoft.AspNetCore.Identity;

namespace logistics.Dtos
{
    public class AdminDto
    {
        public string? UserName { get; set; }
        public string? UserPhone { get; set; }
        public string? Password { get; set; }
        public string? PasswordHash { get; set; }
        public string? Email { get; set; }
    }
}

