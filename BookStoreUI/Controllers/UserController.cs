using BLL.Abstractions.ServiceInterfaces;
using Core.DTO_Models;
using Core.Models;
using FluentResults;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreUI.Controllers
{
    [ApiController]
    [Route("api/user")]
    [Produces("application/json")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        [Route("Login")]
        public async Task<ActionResult<User>> SignIn([FromBody] LoginDTO loginPswd)
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

            return Ok(user);
        }

        [HttpPost]
        [Route("Registration")]
        public async Task<ActionResult<User>> Register([FromBody] UserDTO user)
        {
            if (user == null
                || string.IsNullOrWhiteSpace(user.UserName)
                || string.IsNullOrWhiteSpace(user.Email)
                || string.IsNullOrWhiteSpace(user.PhoneNumber)
                || string.IsNullOrWhiteSpace(user.Password)) // check if any values are null or empty
            {
                return BadRequest("User values can't be empty");
            }
            if ( await _userService.RegisterAsync(user))
            {
                return BadRequest();
            }

            return Ok(user); // return jwt token
        }
    }
}
