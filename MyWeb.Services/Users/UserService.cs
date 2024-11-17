using Microsoft.EntityFrameworkCore;
using MyWeb.EntityFramework;
using MyWeb.Shared.GenericInterfaces;
using MyWeb.Shared.Sessions;

namespace MyWeb.Services.Users
{
    public class UserService : IUserService, IGenericService<Core.Administration.Users.User>
    {
        private readonly MyWebDBContext _DbContext;
        private readonly Administration.Users.IUserAdminService _UserAdminService;
        private readonly Administration.UserPermissions.IUserPermissionService _UserPermissionService;
        private readonly IMySession _Session;

        public UserService(
            MyWebDBContext DbContext,
            Administration.Users.IUserAdminService UserAdminService,
            Administration.UserPermissions.IUserPermissionService UserPermissionService,
            IMySession Session
            )
        {
            _DbContext = DbContext;
            _UserAdminService = UserAdminService;
            _UserPermissionService = UserPermissionService;
            _Session = Session;
        }

        public async Task<List<Core.Administration.Users.User>> GetAllUsers(string search)
        {
            return await _DbContext.Users.Where(x => search == null ? true : x.Name == search).ToListAsync();
        }

        public async Task<Core.Administration.Users.User> Get(long id)
        {
            return await _DbContext.Users.Where(x => x.Id == id).FirstAsync();
        }

        public async Task DeleteUser(long id)
        {
            if (id == _Session.UserId.Value)
                throw new UnauthorizedAccessException();
            var item = await Get(id);
            await _UserPermissionService.ClearPermissions(id);
            _DbContext.Users.Remove(item);
            await _DbContext.SaveChangesAsync();
        }

        public async Task<Core.Administration.Users.User> AddOrUpdateUser(Dtos.Users.AddOrEditUserDto input)
        {
            if (input.Id == null)
                return await AddUser(input);
            else
                return await UpdateUser(input);
        }

        private async Task<Core.Administration.Users.User> AddUser(Dtos.Users.AddOrEditUserDto input)
        {
            var user = new Core.Administration.Users.User();

            user.UserName = input.Item.UserName;
            user.Name = input.Item.Name;
            user.Email = input.Item.Email;
            user.IsAdmin = input.Item.IsAdmin;
            user.Password = _UserAdminService.EncryptPassword(input.PlainTextPassword);
            user.UserPermissions = (from x in input.Permissions
                                    select new Core.Administration.UserPermissions.UserPermission
                                    {
                                        Name = x
                                    }).ToList();

            await _DbContext.Users.AddAsync(user);
            await _DbContext.SaveChangesAsync();
            return user;
        }

        private async Task<Core.Administration.Users.User> UpdateUser(Dtos.Users.AddOrEditUserDto input)
        {
            var user = await Get(input.Id.Value);

            user.UserName = input.Item.UserName;
            user.Name = input.Item.Name;
            user.Email = input.Item.Email;
            user.IsAdmin = input.Item.IsAdmin;

            if (input.ChangePassword)
                user.Password = _UserAdminService.EncryptPassword(input.PlainTextPassword);

            user.UserPermissions = (from x in input.Permissions
                                    select new Core.Administration.UserPermissions.UserPermission
                                    {
                                        Name = x
                                    }).ToList();

            _DbContext.Users.Update(user);
            await _DbContext.SaveChangesAsync();
            return user;
        }
    }

    public interface IUserService
    {
        public Task<List<Core.Administration.Users.User>> GetAllUsers(string search);
        public Task<Core.Administration.Users.User> Get(long id);
        public Task<Core.Administration.Users.User> AddOrUpdateUser(Dtos.Users.AddOrEditUserDto input);
        public Task DeleteUser(long id);
    }
}
