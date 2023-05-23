using System;
using System.Security.Cryptography;
using logistics.Data;
using logistics.Interfaces;
using logistics.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace logistics.Repositories
{
    public class AdminRepository : IAdminRepository
    {
        protected readonly WebDbContext _dataContext;
        public AdminRepository(WebDbContext dataContext)
        {
            _dataContext = dataContext;
        }

        public string GetAdminPassword(string userName)
        {
            var admin = _dataContext.Admins.Where(a => a.UserName.Trim().ToUpper() == userName.Trim().ToUpper()).FirstOrDefault();
            return admin.Password.ToString();
        }

        public async Task<bool> RegisterAdmin(Admin admin)
        {
            await _dataContext.Admins.AddAsync(admin);
            return await Save();
        }

        public async Task<bool> Save()
        {
            var saved = await _dataContext.SaveChangesAsync();
            return saved > 0 ? true : false;
        }
    }
}

