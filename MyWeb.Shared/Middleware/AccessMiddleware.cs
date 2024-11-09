using Microsoft.AspNetCore.Http;
using MyWeb.Shared.Sessions;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MyWeb.Shared.Middleware
{
    public class AccessMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IMySession _session;

        public AccessMiddleware(
            RequestDelegate next,
            IMySession session

            )
        {
            _next = next;
            _session = session;
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
                var jwtObj = new JwtSecurityToken(jwtEncodedString: token);
                string userId = jwtObj.Claims.Where(x => x.Type == "UserId").First().Value;
                string userName = jwtObj.Claims.Where(x => x.Type == ClaimTypes.NameIdentifier).First().Value;
                _session.Init(userId, userName);
            }
            // Call the next delegate/middleware in the pipeline.
            await _next(context);
        }
    }
}
