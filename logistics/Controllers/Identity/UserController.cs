using System;
using logistics.Models.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace logistics.Controllers.Identity
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;

        public UserController(
            UserManager<User> userManager,
            RoleManager<Role> roleManager
            )
        {
            this._userManager = userManager;
            this._roleManager = roleManager;
        }


        [HttpGet]
        public IActionResult Get() => Ok(
            _userManager.Users
            .Where(user => user.DeletedAt == null)
            .Select(user => new
            {
                user.Id,
                user.UserName,
                user.Email,
                user.PhoneNumber,
                user.CreatedAt,
            }));

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            if (string.IsNullOrEmpty(id.ToString()))
                return BadRequest(new string[] { "Empty parameter!" });

            return Ok(_userManager.Users
                .Where(user => user.Id == id)
                .Where(user => user.DeletedAt == null)
                .Select(user => new
                {
                    user.Id,
                    user.UserName,
                    user.Email,
                    user.PhoneNumber,
                    user.CreatedAt
                })
                .FirstOrDefault());
        }

        //public async Task<IActionResult>

        [HttpPost("{id}/role/{roleId}")]
        public async Task<IActionResult> InsertWithRole(string id, string roleId)
        {
            try
            {
                (User user, Role role) = await ValidateUserAndRole(id, roleId);

                var result = await _userManager.AddToRoleAsync(user, role.Name).ConfigureAwait(false);
                if (!result.Succeeded)
                    return StatusCode(500, "Failed to add role");

                return Ok("Role added successfully");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}/role/{roleId}")]
        public async Task<IActionResult> DeleteWithRole(string id, string roleId)
        {
            try
            {
                (User user, Role role) = await ValidateUserAndRole(id, roleId);

                var result = await _userManager.RemoveFromRoleAsync(user, role.Name).ConfigureAwait(false);
                if (!result.Succeeded)
                    return StatusCode(500, "Failed to remove role");

                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id).ConfigureAwait(false);

            if (user == null)
                return BadRequest("user not found");

            if (user.DeletedAt != null)
                return BadRequest("user deleted");

            user.DeletedAt = DateTime.UtcNow;
            var result = await _userManager.UpdateAsync(user).ConfigureAwait(false);
            if (!result.Succeeded)
                return StatusCode(500, "internal error");

            return NoContent();
        }

        private async Task<(User, Role)> ValidateUserAndRole(string userId, string roleId)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(roleId))
                throw new ArgumentException("Empty parameter!");

            User user = await _userManager.FindByIdAsync(userId).ConfigureAwait(false);
            Role role = await _roleManager.FindByIdAsync(roleId).ConfigureAwait(false);

            if (user == null)
                throw new ArgumentException("User not found");

            if (role == null)
                throw new ArgumentException("Role not found");

            return (user, role);
        }


    }
}

