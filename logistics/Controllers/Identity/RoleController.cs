using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using logistics.Models.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using static logistics.Models.AuthViewModel;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace logistics.Controllers.Identity
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {

        private readonly RoleManager<Role> _roleManager;

        public RoleController(RoleManager<Role> roleManager)
        {
            _roleManager = roleManager;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Role>), 200)]
        public IActionResult GetAll()
        {
            try
            {
                return Ok(_roleManager.Roles.Select(role => new { role.Id, role.Name }));
            }
            catch (Exception e)
            {
                return StatusCode(500);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(RoleModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.Values.Select(x => x.Errors.FirstOrDefault()?.ErrorMessage));

            Role role = new Role { Name = model.Name };

            IdentityResult result = await _roleManager.CreateAsync(role).ConfigureAwait(false);
            if (!result.Succeeded)
                return BadRequest(result.Errors.FirstOrDefault()?.Description);

            return Ok("successfully");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] RoleModel model)
        {
            var role = await _roleManager.FindByIdAsync(id.ToString()).ConfigureAwait(false);

            if (role == null)
                return BadRequest("角色不存在");
            role.Name = model.Name;

            var result = await _roleManager.UpdateAsync(role).ConfigureAwait(false);

            if (!result.Succeeded)
                return BadRequest(result.Errors.FirstOrDefault()?.Description);

            return Ok("successfully");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            if (string.IsNullOrEmpty(id.ToString()))
                return BadRequest("无法完成请求");

            var role = await _roleManager.FindByIdAsync(id.ToString()).ConfigureAwait(false);

            if (role == null)
                return BadRequest("角色不存在");

            var result = await _roleManager.DeleteAsync(role).ConfigureAwait(false);
            if (!result.Succeeded)
                return StatusCode(500, result.Errors.Select(x => x.Description));

            return Ok("successfully");
        }
    }
}

