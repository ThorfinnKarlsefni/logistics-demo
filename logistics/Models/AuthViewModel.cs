using System;
using System.ComponentModel.DataAnnotations;

namespace logistics.Models
{
    public class AuthViewModel
    {
        public class TokenModel
        {
            public bool? HasVerifiedEmail { get; set; }
            public bool? TFAEnabled { get; set; }
            public string? Token { get; set; }
        }

        public class RoleModel
        {
            public int Id { get; set; }
            [Required]
            public string? Name { get; set; }
        }

        public class UserRoleViewModel
        {
            public string Id { get; set; }
            public string RoleId { get; set; }
            public string? Token { get; set; }
        }

        public class RegisterUserViewModel
        {
            public string? UserName { get; set; }
            public string? NormalizedUserName { get; set; }
            [EmailAddress]
            public string? Email { get; set; }
            public string? PhoneNumber { get; set; }
            [Required]
            public string? Password { get; set; }

            [Required]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string? ConfirmPassword { get; set; }
        }

        public class LoginUserViewModel
        {
            public string? UserName { get; set; }
            [EmailAddress]
            public string? Email { get; set; }
            public string? Password { get; set; }

            public bool RememberMe { get; set; }
        }

        public class ResetPasswordViewModel
        {
            [Required]
            public string? UserId { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            public string? Password { get; set; }

            [Required]
            [DataType(DataType.Password)]
            [Compare("Password", ErrorMessage = "两次密码不一致")]
            public string? ConfirmPassword { get; set; }

            public string? Code { get; set; }
        }

        public class ForgotPasswordViewModel
        {
            [Required]
            public string? UserId { get; set; }
            [Required]
            [DataType(DataType.Password)]
            public string? Password { get; set; }
            [Required]
            [DataType(DataType.Password)]
            [Compare("Password", ErrorMessage = "两次密码不一致")]
            public string? ConfirmPassword { get; set; }

            public string? Code { get; set; }
        }

    }
}

