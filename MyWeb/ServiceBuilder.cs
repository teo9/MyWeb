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
            services.AddScoped<Services.Administration.Users.IUserAdminService, Services.Administration.Users.UserAdminService>();
            services.AddScoped<Services.Administration.UserPermissions.IUserPermissionService, Services.Administration.UserPermissions.UserPermissionService>();
            services.AddScoped<Shared.Sessions.IMySession,Shared.Sessions.MySession>();
            services.AddScoped<Shared.Permissions.IPermissionChecker, Shared.Permissions.PermissionChecker>();
            services.AddScoped<Services.Users.IUserService, Services.Users.UserService>();
            services.AddScoped<Services.Users.IUserService, Services.Users.UserService>();
        }
    }
}
