using ChatApp.Server.Models;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace ChatApp.Server.Services
{
    public class JWTService
    {
        private readonly IConfiguration _config;
        private readonly SymmetricSecurityKey _jwtKey;

        public JWTService(IConfiguration config)
        {
            _config = config;
            _jwtKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config["JWT:Key"]));
        }
        public string GetClaim(string token,string claimType)
        {
            var tokenHandler = new JwtSecurityToken(token);
            return tokenHandler.Claims.FirstOrDefault(p => p.Type == claimType).Value;
        }
        public string CreateJWT(User user)
        {
            var claims = new List<Claim>()
            {
                new(ClaimTypes.NameIdentifier,user.Id),
                new(ClaimTypes.Email,user.Email),
                new(ClaimTypes.GivenName,user.FirstName),
                new(ClaimTypes.Surname,user.LastName)
            };
            var credentials = new SigningCredentials(_jwtKey, SecurityAlgorithms.HmacSha256);
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(int.Parse(_config["JWT:ExpiresInDays"])),
                SigningCredentials = credentials,
                Issuer = _config["JWT:Issuer"]
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var jwt = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(jwt);
        }
    }
}
