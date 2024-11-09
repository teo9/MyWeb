using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyWeb.Dtos.Login;
using MyWeb.Services.Administration.Users;
using MyWeb.Shared.Sessions;

namespace MyWeb.Controllers.Login
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class LoginController : Controller
    {
        private readonly IUserService _userService;
        private readonly IMySession _mySession;

        public LoginController(
            IUserService userService,
            IMySession mySession
            )
        {
            _userService = userService;
            _mySession = mySession;
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginInputDto input)
        {
            if (string.IsNullOrWhiteSpace(input.UserName) || string.IsNullOrWhiteSpace(input.Password))
                return Unauthorized("Invalid user credentials.");

            var user = await _userService.GetLoggingUser(input.UserName, input.Password);
            if (user == null)
                return Unauthorized("Invalid user credentials.");

            string token = _userService.GetJWTToken(user);

            return Ok(token);
        }

        [HttpPost]
        [Authorize]
        public void LogOut()
        {

        }
    }
}
