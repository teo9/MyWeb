using Microsoft.AspNetCore.Mvc;
using MyWeb.Dtos.Users;
using MyWeb.Services.Users;
using MyWeb.Shared.Permissions;

namespace MyWeb.Controllers.Users
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class UserController : Controller
    {

        private readonly IUserService _UserService;

        public UserController(
            IUserService userService
            )
        {
            _UserService = userService;
        }

        [HttpPost]
        [MyAuthorize(AppPermissions.Administration_Users)]
        public async Task<List<UserDto>> GetUsers(UserFilterDto input)
        {
            var objDtos = await _UserService.GetAllUsers(input.SearchName);

            List<UserDto> results = (from x in objDtos
                                     select new UserDto
                                     {
                                         Id = x.Id,
                                         UserName = x.UserName,
                                         Name = x.Name,
                                         Email = x.Email,
                                         IsAdmin = x.IsAdmin
                                     })
                                     .ToList();
            return results;
        }


        [HttpPost]
        [MyAuthorize(AppPermissions.Administration_Users)]
        public async Task AddOrUpdateUser(AddOrEditUserDto input)
        {
            await _UserService.AddOrUpdateUser(input);
        }

        [HttpPost]
        [MyAuthorize(AppPermissions.Administration_Users)]
        public async Task RemoveUser(long id)
        {
            await _UserService.DeleteUser(id);
        }
    }
}
