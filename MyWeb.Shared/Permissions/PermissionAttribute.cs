

using MyWeb.Shared.Sessions;

namespace MyWeb.Shared.Permissions
{
     
    public class MyAuthorizeAttribute : Attribute
    {
          string[] Permissions {get; set;}

          public MyAuthorizeAttribute(params string[] permissions){
               this.Permissions = permissions;
          }
    }
}