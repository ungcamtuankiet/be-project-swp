using be_artwork_sharing_platform.Core.Dtos.Auth;
using be_artwork_sharing_platform.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Net;

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

        [HttpPost]
        [Route("forgot-passworda")]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            // Kiểm tra xem email có tồn tại trong cơ sở dữ liệu hay không
            var user = _authService.GetUserByEmailAsync(email);
            if (user is null)
            {
                return NotFound("Email không tồn tại trong hệ thống");
            }
            // Nếu không tồn tại, trả về lỗi
            // Nếu tồn tại, tạo mã code 6 chữ số
            string verificationCode = GenerateVerificationCode();

            // Gửi email chứa mã code tới địa chỉ email của người dùng
            bool isEmailSent = await SendVerificationCodeByEmail(email, verificationCode);

            if (isEmailSent)
            {
                return Ok(new { message = "Mã code đã được gửi đến email của bạn." });
            }
            else
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new { message = "Có lỗi xảy ra khi gửi email." });
            }
        }

        // Hàm tạo mã code 6 chữ số ngẫu nhiên
        private string GenerateVerificationCode()
        {
            Random random = new Random();
            return random.Next(100000, 999999).ToString();
        }

        // Hàm gửi email chứa mã code tới địa chỉ email của người dùng
        private async Task<bool> SendVerificationCodeByEmail(string email, string verificationCode)
        {
            try
            {
                string smtpServer = "smtp.gmail.com";
                int smtpPort = 587; // Port của SMTP server
                string smtpUsername = "ungcamtuankiet94@gmail.com";
                string smtpPassword = "Kiet764285199";


                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress("ungcamtuankiet94@gmail.com");
                mailMessage.To.Add(email);
                mailMessage.Subject = "Password Reset Verification Code";
                mailMessage.Body = $"Your verification code is: {verificationCode}";

                SmtpClient smtpClient = new SmtpClient(smtpServer, smtpPort);
                smtpClient.Credentials = new NetworkCredential(smtpUsername, smtpPassword);
                smtpClient.UseDefaultCredentials = false;
                smtpClient.EnableSsl = true;


                await smtpClient.SendMailAsync(mailMessage);
                return true;
            }
            catch (Exception ex)
            {
                // Xử lý lỗi khi gửi email
                Console.WriteLine(ex.Message);
                return false;
            }
        }


    }
}
