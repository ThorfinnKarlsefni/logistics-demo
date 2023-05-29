using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace logistics.Models.Identity
{
    public class User : IdentityUser<long>
    {
        public DateTime CreatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}