using MyWeb.Shared.Sessions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWeb.Shared.Permissions
{
    public class PermissionChecker : IPermissionChecker
    {
        private readonly IMySession _Session;

        public PermissionChecker(
            IMySession Session
            )
        {
            _Session = Session;
        }

        public bool Check(MyAuthorizeAttribute attribute)
        {
            if (_Session.Permissions.Any(x => x == AppPermissions.IsAdmin))
                return true;
            foreach (var permission in attribute.Permissions)
            {
                if (permission != null && _Session.Permissions.Any(x => x == permission))
                    return true;
            }
            return false;
        }
    }

    public interface IPermissionChecker
    {
        public bool Check(MyAuthorizeAttribute attribute);
    }
}
