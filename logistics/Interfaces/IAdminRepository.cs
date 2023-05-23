using System;
using System.Security.Cryptography;
using logistics.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace logistics.Interfaces
{
    public interface IAdminRepository
    {
        Task<bool> RegisterAdmin(Admin adimn);
        string GetAdminPassword(string userName);
        Task<bool> Save();
    }
}

