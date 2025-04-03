using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Service.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Service.Services
{
    public class JwtService : IJwtService
    {
        private IList<string> _roles;
        private IList<Claim> _claims;
        private JwtOptions _jwtoptions;

        public JwtService(IConfiguration configuration)
        {
            _roles = new List<string>();
            _claims = new List<Claim>();
            _jwtoptions = configuration.GetSection("Jwt").Get<JwtOptions>() ?? new JwtOptions();
        }

        public string GenerateToken(string infos)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtoptions.Key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            this._claims.Add(new Claim(ClaimTypes.NameIdentifier, infos));

            foreach (var role in _roles)
            {
                this._claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var token = new JwtSecurityToken(
                issuer: _jwtoptions.Issuer,
                audience: _jwtoptions.Audience,
                claims: _claims,
                expires: DateTime.Now.AddMinutes(_jwtoptions.lifetime),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        public void AddRoles(params string[] roles)
        {
            foreach (var role in roles)
            {
                this._roles.Add(role);
            }
        }
    }
}
