using ChatApp.Server.DTOs.Account;
using ChatApp.Server.Models;
using ChatApp.Server.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text;

namespace ChatApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly JWTService _jwtService;
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly EmailService _emailService;
        private readonly IConfiguration _config;

        public AccountController(JWTService jwtService,
            SignInManager<User> signInManager,
            UserManager<User> userManager,
            EmailService emailService,
            IConfiguration config)
        {
            _jwtService = jwtService;
            _signInManager = signInManager;
            _userManager = userManager;
            _emailService = emailService;
            _config = config;
        }
        [HttpGet("Users")]
        public ActionResult<IEnumerable<User>> GetUsers(){
            return Ok(_userManager.Users);
        }

        [Authorize]
        [HttpGet("refresh-user-token")]
        public async Task<ActionResult<UserDto>> RefreshUserToken()
        {
            var user = await _userManager.FindByEmailAsync(User.FindFirst(ClaimTypes.Email)?.Value);
            return CreateApplicationUserDto(user);
        }

        [HttpPost("Login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto model)
        { 
            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user is null)
                return Unauthorized("Invalid username or password");
            if (!user.EmailConfirmed)
                return Unauthorized("Please confirm your email");

            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
            if (!result.Succeeded)
                return Unauthorized("Invalid username or password");



            return CreateApplicationUserDto(user);

        }
        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterDto model)
        {
            if (await CheckEmailExistsAsync(model.Email))
            {
                return BadRequest($"An existing account is using {model.Email} email address.Please try with another email address");
            }
            var userToAdd = new User
            {
                FirstName = model.FirstName.ToLower(),
                LastName = model.LastName.ToLower(),
                Email = model.Email.ToLower(),
                UserName = model.Email.ToLower(),
            };
            var result = await _userManager.CreateAsync(userToAdd, model.Password);
            if (!result.Succeeded)
                return BadRequest(result.Errors);


            try
            {
                if (await SendConfirmEmailAsync(userToAdd))
                    return Ok(new JsonResult(new {title="Account created"
                        ,message="Your account has been created,please confirm your email"
                    }));
            }
            catch (Exception)
            {
                return BadRequest("Failedto send email.Please contact email");
            }
            return Ok(new JsonResult(
                new { title = "Account Created",message = "Your account has been created,you can login" })); 
        
        }
        [HttpDelete("delete/{email}")]
        public async Task<IActionResult> DeleteUser(string email) {
            var user = await _userManager.FindByEmailAsync(email);
            return Ok(await _userManager.DeleteAsync(user));
        }
        [HttpPut("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(ConfirmEmailDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null) return Unauthorized("This email has not been registered yet");
            if (user.EmailConfirmed) return BadRequest("Your account has already been confirmed");
            try
            {
                var decodedTokenBytes = WebEncoders.Base64UrlDecode(model.Token);
                var decodedToken = Encoding.UTF8.GetString(decodedTokenBytes);
                var result = await _userManager.ConfirmEmailAsync(user, decodedToken);
                if (result.Succeeded)
                    return Ok(new JsonResult(new { title = "Email confirmed", message = "Your email address is confirmed.You can login" }));
                return BadRequest("Invalid token");
            }
            catch (Exception)
            {

                return BadRequest("Invalid token");

            }
        }
        [HttpPut("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null) return BadRequest("Invalid email");
            if (!user.EmailConfirmed) return BadRequest("Please confirm your email address");
            if (await _userManager.CheckPasswordAsync(user, model.NewPassword)) return BadRequest("The new password should be unique from old password");
            try
            {
                var decodedTokenBytes = WebEncoders.Base64UrlDecode(model.Token);
                var decodedToken = Encoding.UTF8.GetString(decodedTokenBytes);
                var result = await _userManager.ResetPasswordAsync(user, decodedToken,model.NewPassword);
                if (result.Errors.Any()) return BadRequest(result.Errors);
                if (result.Succeeded)
                    return Ok(new JsonResult(new { title = "Password is changed", message = "Please login to your account" }));
                return BadRequest("Invalid token");
            }
            catch (Exception)
            {
                return BadRequest("Invalid token");

            }
        }

        [HttpPost("resend-email-confirmation-link/{email}")]
        public async Task<IActionResult> ResendEmailConfirmationLink(string email)
        {
            Console.WriteLine(HttpContext.Request.Body);
            if (string.IsNullOrWhiteSpace(email)) return BadRequest("Invalid email");
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) return Unauthorized("This user has not been registered yet");
            if (user.EmailConfirmed) return BadRequest("Your account has already been confirmed.Please login to your account");
            try
            {
                if (await SendConfirmEmailAsync(user))
                    return Ok(new JsonResult(new { title = "Confirmation link sent", message = "Please confirm your email address" }));
                return BadRequest("Failed to send email.Please contact admin");
            }
            catch (Exception)
            {
                return BadRequest("Failed to send email.Please contact admin");
                
            }
        }

        [HttpPost("forgot-username-or-password/{email}")]
        public async Task<IActionResult> ForgotUsernameOrPassword(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return BadRequest("Invalid email");
            var user = await _userManager.FindByEmailAsync(email);
            
            if (user == null) return Unauthorized("This email has not been registered yet");
            if (!user.EmailConfirmed) return Unauthorized("This email has not been confirmed yet");
            try
            {
                if(await SendForgotUsernameOrPasswordEmailAsync(user))
                {
                    return Ok(new JsonResult(new { title = "Forgot username or password sent", message = "Please check your email" }));
                }
                return BadRequest("Failed to send email");
            }
            catch (Exception)
            {

                return BadRequest("Failed to send email");
            }
        }

        

        #region Private
        private async Task<bool> CheckEmailExistsAsync(string email)
        {
            return await _userManager.Users.AnyAsync(x => x.Email == email.ToLower());
        }
        
        private UserDto CreateApplicationUserDto(User user)
        {
            return new UserDto()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                JWT = _jwtService.CreateJWT(user)
            };
        }

       

        private async Task<bool> SendConfirmEmailAsync(User user)
        {
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
            var url = $"{_config["JWT:ClientUrl"]}/{_config["Email:ConfirmationEmailPath"]}?token={token}&email={user.Email}";
            var body = $"""
                    <h3>Hello:{user.FirstName} {user.LastName}</h3>
                    <p>Please confirm your email address by clicking on the following link</p>
                    <p><a href="{url}">Click here</a></p>
                    <h3>Thank you,{_config["Email:From"]}</h3>
                """;
            var emailSend = new EmailSendDto(user.Email,"Confirm your email",body);
            return await _emailService.SendEmailAsync(emailSend);
        }
        private async Task<bool> SendForgotUsernameOrPasswordEmailAsync(User user)
        {
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
            var url = $"{_config["JWT:ClientUrl"]}/{_config["Email:ResetPasswordPath"]}?token={token}&email={user.Email}";
            var body = $"""
                    <h3>Hello:{user.FirstName} {user.LastName}</h3>
                    <h3>Your username:{user.UserName}</h3>
                    <p>Please reset your password by clicking on the following link</p>
                    <p><a href="{url}">Click here</a></p>
                    <h3>Thank you,{_config["Email:From"]}</h3>
                """;
            var emailSend = new EmailSendDto(user.Email, "Forgot username or password", body);
            return await _emailService.SendEmailAsync(emailSend);
        }
        #endregion
    }
}
