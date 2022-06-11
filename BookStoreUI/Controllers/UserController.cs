using BLL.Abstractions.ServiceInterfaces;
using Core.DTO_Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace BookStoreUI.Controllers
{
    [ApiController]
    [Route("api/user")]
    [Produces("application/json")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;
        public UserController(IUserService userService, IConfiguration configuration)
        {
            _userService = userService;
            _configuration = configuration;
        }

        [HttpPost]
        [Route("Login")]
        public async Task<ActionResult<UserAfterLogin>> SignIn([FromBody] LoginDTO loginPswd)
        {
            if (loginPswd == null
                || string.IsNullOrWhiteSpace(loginPswd.Login)
                || string.IsNullOrWhiteSpace(loginPswd.Password))
            {
                return BadRequest("Invalid value provided");
            }

            var user = await _userService.SignInAsync(loginPswd.Login, loginPswd.Password);

            if (user == null)
            {
                return BadRequest("User was not found");
            }
            var res = new UserAfterLogin
            {
                UserId = user.UserId,
                Token = GenerateJwtToken(user.UserId),
                CancelDate = DateTime.UtcNow.AddDays(1).ToString("yyyy-MM-ddTHH:mm:ss"),
                Role = user.Role,
            };
            return Ok(res);
        }

        private string GenerateJwtToken(int id)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, id.ToString()),
                new Claim(ClaimTypes.SerialNumber, new Guid().ToString()),
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("AppSetings:Token").Value));

            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                signingCredentials: cred,
                expires: DateTime.UtcNow.AddDays(1)
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [HttpPost]
        [Route("Registration")]
        public async Task<ActionResult<bool>> Register([FromBody] UserDTO user)
        {
            if (user == null
                || string.IsNullOrWhiteSpace(user.UserName)
                || string.IsNullOrWhiteSpace(user.Email)
                || string.IsNullOrWhiteSpace(user.PhoneNumber)
                || string.IsNullOrWhiteSpace(user.Password)) // check if any values are null or empty
            {
                return BadRequest("User values can't be empty");
            }
            if (! await _userService.RegisterAsync(user))
            {
                return BadRequest("Cannot register u(");
            }

            return Ok(true); // return jwt token
        }

        [HttpGet]
        [Route("RefreshToken")]
        public async Task<ActionResult> RefreshToken(int userId)
        {
            return Ok(new
            {
                Token = GenerateJwtToken(userId),
                CancelDate = DateTime.UtcNow.AddDays(1).ToString("yyyyMMddTHH:mm:ss"),
            });
        }
    }
}
