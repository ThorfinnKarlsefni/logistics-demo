using System;
namespace logistics.Models
{
    public class AdminToken
    {
        public int AdminId { get; set; }
        public string? Name { get; set; }
        public string? LoginProvider { get; set; }
        public string? Value { get; set; }
    }
}

