using System;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;
namespace logistics.Models
{
    public class Admin
    {
        public long Id { get; set; }
        public string? UserName { get; set; }
        public string? UserPhone { get; set; }
        public string? Password { get; set; }
        public string? PasswordHash { get; set; }
        public string? Email { get; set; }
        public string? Token { get; set; }
        public string? RefreshToken { get; set; }
        //public bool Lock { get; set; } = false;
        public DateTime CreatedAt { get; init; }
        public DateTime? DeletionTime { get; private set; }
    }
}