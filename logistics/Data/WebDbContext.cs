using System;
using logistics.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace logistics.Data
{
    public class WebDbContext : IdentityDbContext<User, Role, long, UserClaim, UserRole, UserLogin, RoleClaim, UserToken>
    {
        protected readonly IConfiguration _configuration;

        public WebDbContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            // connect to postgres with connection string from app settings
            options.UseNpgsql(_configuration.GetConnectionString("WebApiDatabase"));
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.ApplyConfiguration(new AdminEntityTypeConfiguration());
            //modelBuilder.ApplyConfiguration(new RoleEntityTypeConfiguration());
            //modelBuilder.Entity<User>().ToTable("Users");
            //modelBuilder.Entity<Role>().ToTable("Roles");
            //modelBuilder.Entity<UserRole>().ToTable("UserRoles").HasNoKey();
            //modelBuilder.Entity<UserLogin>().ToTable("UserLogins");
            //modelBuilder.Entity<UserClaim>().ToTable("UserClaims");
            //modelBuilder.Entity<RoleClaim>().ToTable("RoleClaims");
            //modelBuilder.Entity<UserToken>().ToTable("UserTokens");

            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<Role>().ToTable("Roles");
            modelBuilder.Entity<UserRole>().ToTable("UserRoles").HasKey(ur => new { ur.UserId, ur.RoleId });
            modelBuilder.Entity<UserLogin>().ToTable("UserLogins").HasKey(ul => new { ul.LoginProvider, ul.ProviderKey });
            modelBuilder.Entity<UserClaim>().ToTable("UserClaims");
            modelBuilder.Entity<RoleClaim>().ToTable("RoleClaims");
            modelBuilder.Entity<UserToken>().ToTable("UserTokens").HasKey(ut => new { ut.UserId, ut.LoginProvider, ut.Name });
        }
    }
}

