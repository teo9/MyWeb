using MyWeb.Shared.Entity;

namespace MyWeb.Core.Administration.UserPermissions
{
    public class UserPermission : MyEntity
    {
        public string Name { get; set; }
        public bool IsStatic { get; set; }
    }
}
