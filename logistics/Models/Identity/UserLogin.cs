using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace logistics.Models.Identity
{
    public class UserLogin : IdentityUserLogin<long>
    {
        [Key]
        public long Id { get; set; }
    }
}

