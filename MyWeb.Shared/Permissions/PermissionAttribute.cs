

using MyWeb.Shared.Sessions;

namespace MyWeb.Shared.Permissions
{
     
    public class MyAuthorizeAttribute : Attribute
    {
          public string[] Permissions {get; private set;}

          public MyAuthorizeAttribute(params string[] permissions){
               this.Permissions = permissions;
          }
    }
}