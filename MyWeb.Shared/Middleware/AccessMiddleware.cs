using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using MyWeb.Shared.Permissions;
using MyWeb.Shared.Sessions;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;

namespace MyWeb.Shared.Middleware
{
    public class AccessMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IMySession _session;
        private readonly IPermissionChecker _PermissionChecker;

        public AccessMiddleware(
            RequestDelegate next,
            IMySession session,
            IPermissionChecker PermissionChecker
            )
        {
            _next = next;
            _session = session;
            _PermissionChecker = PermissionChecker;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var cultureQuery = context.Request.Query["culture"];
            if (!string.IsNullOrWhiteSpace(cultureQuery))
            {
                var culture = new CultureInfo(cultureQuery);
                CultureInfo.CurrentCulture = culture;
                CultureInfo.CurrentUICulture = culture;
            }

            if (context.Request.Headers.Any(x => x.Key == "Authorization"))
            {
                string token = context.Request.Headers.Where(x => x.Key == "Authorization").Select(x => x.Value).First();
                token = token.Replace("Bearer ", "");
                try
                {
                    var jwtObj = new JwtSecurityToken(jwtEncodedString: token);
                    string userId = jwtObj.Claims.Where(x => x.Type == "UserId").First().Value;
                    string userName = jwtObj.Claims.Where(x => x.Type == ClaimTypes.NameIdentifier).First().Value;
                    List<string> permissions = jwtObj.Claims.Where(x => x.Type == ClaimTypes.Role).Select(x => x.Value).ToList();

                    _session.Init(userId, userName, permissions);
                }
                catch { }
            }

            var endpoint = context.Features.Get<IEndpointFeature>()?.Endpoint;
            var attribute = endpoint?.Metadata.GetMetadata<MyAuthorizeAttribute>();

            if (attribute != null && !_PermissionChecker.Check(attribute))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return;
            } 

            // Call the next delegate/middleware in the pipeline.
            await _next(context);
        }
    }
}
