using System;
using System.Security.Cryptography;
using AutoMapper;
using FluentValidation;
using logistics.Dtos;
using logistics.Helpers;
using logistics.Interfaces;
using logistics.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace logistics.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : Controller
    {
        public readonly IAdminRepository _adminRepository;
        public readonly IMapper _mapper;

        public AdminController(IAdminRepository adminRepository, IMapper mapper)
        {
            _adminRepository = adminRepository;
            _mapper = mapper;
        }

        [HttpPost("Register")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Register(IValidator<AdminDto> validator, [FromBody] AdminDto adminRegister)
        {
            var validationResult = await validator.ValidateAsync(adminRegister);

            if (!validationResult.IsValid)
            {
                return StatusCode(403, validationResult.Errors[0].ErrorMessage);
            }

            adminRegister.Password = HashPassword(adminRegister.Password);

            var admin = _mapper.Map<Admin>(adminRegister);
            if (!await _adminRepository.RegisterAdmin(admin))
            {
                ModelState.AddModelError("", "system error");
                return StatusCode(500, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok("successfull");
        }

        [HttpPost("Login")]
        [ProducesResponseType(200)]
        public IActionResult AdminLogin([FromBody] AdminDto admin)
        {
            if (admin.UserName == null || admin.Password == null)
                return StatusCode(403, "用户名或密码不为空");
            var hashPassword = HashPassword(admin.Password);
            Console.WriteLine(_adminRepository.GetAdminPassword(admin.Password));
            Console.WriteLine(hashPassword);
            //PasswordHasher<Tuser>.;
            return Ok();
        }

        protected static string HashPassword(string password)
        {

            byte[] salt = RandomNumberGenerator.GetBytes(128 / 8);
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password!,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));

            return hashed;

        }
    }


}


