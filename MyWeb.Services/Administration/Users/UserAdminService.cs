using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MyWeb.EntityFramework;
using MyWeb.Shared.Permissions;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace MyWeb.Services.Administration.Users
{
    /*
     migrationBuilder.Sql("INSERT INTO [Users] (UserName, Name, Email, Password) VALUES ('admin','Admin','admin@myweb.it','8c6976e5b5410415bde908bd4dee15dfb167a9c873fc4bb8a81f6f2ab448a918');");
     
    admin admin
     */

    public class UserAdminService : IUserAdminService
    {
        private readonly MyWebDBContext _DbContext;
        private readonly IConfiguration _configuration;

        public UserAdminService(
             MyWebDBContext DbContext,
             IConfiguration configuration
            )
        {
            _DbContext = DbContext;
            _configuration = configuration;
        }

        public async Task<Core.Administration.Users.User?> GetLoggingUser(string username, string password)
        {
            string encPassword = EncryptPassword(password);
            var user = await _DbContext.Users.Include(x => x.UserPermissions).FirstOrDefaultAsync(x => x.UserName == username && x.Password == encPassword);
            return user;
        }

        public string EncryptPassword(string plainText)
        {
            var crypt = new System.Security.Cryptography.SHA256Managed();
            var hash = new System.Text.StringBuilder();
            byte[] crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(plainText));
            foreach (byte theByte in crypto)
            {
                hash.Append(theByte.ToString("x2"));
            }
            return hash.ToString();
        }

        public string GetJWTToken(Core.Administration.Users.User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            // Sets up the signing credentials using the above security key and specifying the HMAC SHA256 algorithm.
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            // Defines a set of claims to be included in the token.
            var claims = new List<Claim>
            {
                // Custom claim using the user's ID.
                new Claim("UserId", user.Id.ToString()), 
                // Standard claim for user identifier, using username.
                new Claim(ClaimTypes.NameIdentifier, user.UserName),
                // Standard claim for user's email.
                new Claim(ClaimTypes.Email, user.Email),
                // Standard JWT claim for subject, using user ID.
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString())
            };
            // Adds a role claim for each role associated with the user.
            user.UserPermissions.ToList().ForEach(role => claims.Add(new Claim(ClaimTypes.Role, role.Name)));

            if (user.IsAdmin)
                claims.Add(new Claim(ClaimTypes.Role, AppPermissions.IsAdmin));

            // Creates a new JWT token with specified parameters including issuer, audience, claims, expiration time, and signing credentials.
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(int.Parse(_configuration["Jwt:ExpirationHours"])), // Token expiration set to 1 hour from the current time.
                signingCredentials: credentials);
            // Serializes the JWT token to a string and returns it.
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

    public interface IUserAdminService
    {
        Task<Core.Administration.Users.User?> GetLoggingUser(string username, string password);
        public string EncryptPassword(string plainText);
        public string GetJWTToken(Core.Administration.Users.User user);
    }
}
