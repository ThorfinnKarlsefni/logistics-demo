using System.Security.Claims;
using logistics.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using static logistics.Models.AuthViewModel;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using logistics.Interfaces.Settings;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.WebUtilities;

namespace logistics.Controllers.Identity
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IJwtSettings _jwt;

        public AuthController(
            UserManager<User> userManager,
            IJwtSettings jwt
            )
        {
            _userManager = userManager;
            _jwt = jwt;
        }

        [HttpPost("Register")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Register([FromBody] RegisterUserViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.Values.Select(x => x.Errors.FirstOrDefault()?.ErrorMessage));

            User user = new User
            {
                UserName = model.UserName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
            };

            user.CreatedAt = DateTime.UtcNow;

            var result = await _userManager.CreateAsync(user, model.Password).ConfigureAwait(false);

            if (!result.Succeeded)
                return BadRequest(result.Errors.Select(x => x.Description));

            return Ok("successfully");

        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginUserViewModel model)
        {

            User user = await _userManager.FindByNameAsync(model.UserName).ConfigureAwait(false);

            if (user == null)
                return BadRequest(new string[] { "无效认证信息" });

            //if (user.LockoutEnabled)
            //    return BadRequest(new string[] { "用户已锁定 请联系管理员" });

            if (await _userManager.CheckPasswordAsync(user, model.Password).ConfigureAwait(false))
            {
                TokenModel tokenModel = new TokenModel();
                if (user.TwoFactorEnabled)
                {
                    return Ok(tokenModel);
                }
                else
                {
                    JwtSecurityToken jwtSecurityToken = await CreateJwtToken(user, model.RememberMe).ConfigureAwait(false);
                    tokenModel.TFAEnabled = false;
                    tokenModel.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
                    return Ok(tokenModel);
                }
            }

            return BadRequest(new string[] { "用户名或密码错误" });
        }

        [HttpGet("Logout")]
        public async Task<IActionResult> Logout()
        {
            return Ok();
        }

        [HttpPost("FotgotPassword")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.Values.Select(x => x.Errors.FirstOrDefault()?.ErrorMessage));

            User user = await _userManager.FindByIdAsync(model.UserId).ConfigureAwait(false);

            if (user == null)
                return BadRequest("用户不存在");

            IdentityResult result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);

            if (!result.Succeeded)
                return BadRequest("系统错误");

            return Ok("successfully");
        }

        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.Values.Select(x => x.Errors.FirstOrDefault()?.ErrorMessage));

            var user = await _userManager.FindByIdAsync(model.UserId).ConfigureAwait(false);

            if (user == null)
                return BadRequest(new string[] { "用户不存在" });

            string code = await _userManager.GeneratePasswordResetTokenAsync(user).ConfigureAwait(false);

            IdentityResult result = await _userManager.ResetPasswordAsync(user, code, model.Password).ConfigureAwait(false);
            if (!result.Succeeded)
                return BadRequest(result.Errors.Select(x => x.Description));

            return Ok(result);

        }

        private async Task<JwtSecurityToken> CreateJwtToken(User user, bool rememberMe)
        {
            IList<Claim> userClaims = await _userManager.GetClaimsAsync(user).ConfigureAwait(false);
            IList<string> roles = await _userManager.GetRolesAsync(user).ConfigureAwait(false);

            var roleClaims = new List<Claim>();

            for (int i = 0; i < roles.Count; i++)
            {
                roleClaims.Add(new Claim("roles", roles[i]));
            }

            //string ipAddress = IpHelper.GetIpAddress();

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id.ToString()),
                // should be like this if we have author 1:1 relationship with User table
                // new Claim("aid", user.Author.ApplicationUserId.ToString()), 
                // but because of the VueBoilerplate, we have to check stuff
                //new Claim("aid", user.Author != null ? user.Author.ApplicationUserId.ToString() : "0"),
                //new Claim("ip", ipAddress)
            }
            .Union(userClaims)
            .Union(roleClaims);

            SymmetricSecurityKey symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            SigningCredentials signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(
            issuer: _jwt.Issuer,
            audience: _jwt.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwt.DurationInMinutes + (rememberMe ? _jwt.RememberMeDurationInHours * 60 : 0)),
            signingCredentials: signingCredentials
            );
            return jwtSecurityToken;
        }
    }


}


