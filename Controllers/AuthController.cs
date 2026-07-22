using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using LogisticDesk.Models;

namespace LogisticDesk.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<LogisticDesk.Models.ApplicationUser> _userManager;

        public AuthController(UserManager<LogisticDesk.Models.ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] LogisticDesk.DTOs.Auth.RegisterRequest request)
        {

            var FullName = request.FullName.Trim();
            var LastName = request.LastName.Trim();
            var Email = request.Email.Trim();
            ApplicationUser User = new ApplicationUser
            {
                UserName = Email,
                Email = Email,
                FirstName = FullName,
                LastName = LastName,
                CreatedAt = DateTime.UtcNow
            };
            var result = await _userManager.CreateAsync(User, request.Password);
            if (!result.Succeeded) {
            return BadRequest(new
            {
                message= "User creation failed",
                errors = result.Errors
            });
            }
            return Ok(new
            {
                message = "User Registered Successfully"
            });





        }
    }
}
