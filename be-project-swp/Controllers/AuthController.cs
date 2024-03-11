using be_artwork_sharing_platform.Core.Constancs;
using be_artwork_sharing_platform.Core.Dtos.Auth;
using be_artwork_sharing_platform.Core.Dtos.General;
using be_artwork_sharing_platform.Core.Dtos.User;
using be_artwork_sharing_platform.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace be_artwork_sharing_platform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        //Route -> Seed Roles to DB
        [HttpPost]
        [Route("seed-roles")]
        public async Task<IActionResult> SeedRoles()
        {
            var seedRoles = await _authService.SeedRoleAsync();
            return StatusCode(seedRoles.StatusCode, seedRoles.Message);
        }

        //Route -> Register
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            var registerResult = await _authService.RegisterAsync(registerDto);
            return StatusCode(registerResult.StatusCode, registerResult.Message);
        }

        //Route -> Login
        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<LoginServiceResponceDto>> Login([FromBody] LoginDto loginDto)
        {
            try
            {
                bool checkStatusUser = await _authService.GetStatusUser(loginDto.UserName);
                if(checkStatusUser)
                {
                    var loginResult = await _authService.LoginAsync(loginDto);
                    if (loginResult is null)
                    {
                        return Unauthorized("Your credentials are invalid. Please contact to an Admin");
                    }
                    return Ok(loginResult);
                }
                else
                {
                    return BadRequest("Your account has been locked");
                }
            }
                
            catch
            {
                return BadRequest("Login Failed");
            }
        }

        /*[HttpPost]
        [Route("update-role")]
        [Authorize(Roles = StaticUserRole.ADMIN)]
        public async Task<IActionResult> UpdateRole([FromBody] UpdateRoleDto updateRoleDto)
        {
            var updateRoleResult = await _authService.UpdateRoleAsync(User, updateRoleDto);

            if (updateRoleResult.IsSucceed)
            {
                return Ok(updateRoleResult.Message);
            }
            else
            {
                return StatusCode(updateRoleResult.StatusCode, updateRoleResult.Message);
            }
        }*/

        [HttpPost]
        [Route("me")]
        public async Task<ActionResult<LoginServiceResponceDto>> Me([FromBody] MeDto meDto)
        {
            try
            {
                var me = await _authService.MeAsync(meDto);
                if (me is not null)
                {
                    return Ok(me);
                }
                else
                {
                    return Unauthorized("InvalidToken");
                }
            }
            catch (Exception)
            {
                return Unauthorized("InvalidToken");
            }
        }
    }
}
