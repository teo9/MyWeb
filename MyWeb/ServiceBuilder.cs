using Microsoft.Extensions.DependencyInjection;

namespace MyWeb
{
    public static class ServiceBuilder
    {
        public static void AddMyWebServices(this IServiceCollection services)
        {
            AddAdministrationServices(services);
        }

        private static void AddAdministrationServices(IServiceCollection services)
        {
            services.AddScoped<Services.Administration.Users.IUserService, Services.Administration.Users.UserService>();
            //services.AddScoped<Services.Administration.UserPermissions.IUserPermissionService, Services.Administration.UserPermissions.IUserPermissionService>();
        }
    }
}
