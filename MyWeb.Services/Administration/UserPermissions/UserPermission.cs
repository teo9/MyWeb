using Microsoft.EntityFrameworkCore;
using MyWeb.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWeb.Services.Administration.UserPermissions
{
    public class UserPermissionService : IUserPermissionService
    {
        private readonly MyWebDBContext _DbContext;
        public UserPermissionService(
             MyWebDBContext DbContext
            )
        {
            _DbContext = DbContext;
        }

        public async Task ClearPermissions(long userId)
        { 
            var user = await _DbContext.Users.Include(x => x.UserPermissions).FirstAsync(x => x.Id == userId);

            _DbContext.UserPermissions.RemoveRange(user.UserPermissions);

            await _DbContext.SaveChangesAsync();
        }
    }

    public interface IUserPermissionService
    {
        public Task ClearPermissions(long userId);
    }
}
