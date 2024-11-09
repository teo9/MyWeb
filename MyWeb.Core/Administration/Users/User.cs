using MyWeb.Core.Administration.UserPermissions;
using MyWeb.Shared.Entity;

namespace MyWeb.Core.Administration.Users
{
    public class User : MyEntity
    {
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string UserToken { get; set; }

        public virtual ICollection<UserPermission> UserPermissions { get; set; }
    }
}
