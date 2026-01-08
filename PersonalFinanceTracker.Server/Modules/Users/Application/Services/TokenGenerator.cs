namespace PersonalFinanceTracker.Server.Modules.Users.Application.Services
{
    using Domain;
    using DTOs.Auth;
    using Microsoft.IdentityModel.Tokens;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Security.Cryptography;
    using System.Text;

    public class TokenGenerator
    {
        private readonly string _secretKey;
        private readonly string _issuer;
        private readonly string _audience;
        private readonly int _expirationInMinutes;

        public TokenGenerator(IConfiguration config)
        {
            _secretKey = config["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key is not configured.");
            _issuer = config["Jwt:Issuer"] ?? throw new InvalidOperationException("JWT Issuer is not configured.");
            _audience = config["Jwt:Audience"] ?? throw new InvalidOperationException("JWT Audience is not configured.");
            string expiration = config["Jwt:ExpirationInMinutes"] ?? throw new InvalidOperationException("JWT Audience is not configured.");
            _expirationInMinutes = int.Parse(expiration);
        }

        public string GenerateAccessToken(AppUser user, IEnumerable<string>? roles = null)
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
                expires: DateTime.UtcNow.AddMinutes(_expirationInMinutes),
                signingCredentials: new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GenerateRefreshToken() => Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
    }
}
