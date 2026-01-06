namespace PersonalFinanceTracker.Server.Modules.Users.Application.Services
{
    using Domain;
    using Microsoft.IdentityModel.Tokens;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;

    public class TokenGenerator
    {
        private readonly string _secretKey;
        private readonly string _issuer;
        private readonly string _audience;

        public TokenGenerator(IConfiguration config)
        {
            _secretKey = config["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key is not configured.");
            _issuer = config["Jwt:Issuer"] ?? throw new InvalidOperationException("JWT Issuer is not configured.");
            _audience = config["Jwt:Audience"] ?? throw new InvalidOperationException("JWT Audience is not configured.");
        }

        public string GenerateToken(AppUser user, IEnumerable<string>? roles = null)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Role, roles is not null ? string.Join(", ", roles) : string.Empty),
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName!),
            };

            var token = new JwtSecurityToken(
                issuer: _issuer,
                audience: _audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
