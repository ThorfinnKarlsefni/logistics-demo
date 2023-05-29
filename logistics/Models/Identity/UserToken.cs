using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace logistics.Models.Identity
{
    public class UserToken : IdentityUserToken<long>
    {
        [Key]
        public long Id { get; set; }
    }
}

