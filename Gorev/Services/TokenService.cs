using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Logging;

public class TokenService
{
    private readonly string _key;
    private readonly string _issuer;
    private readonly string _audience;
    private readonly ILogger<TokenService> _logger;

    public TokenService(IConfiguration configuration, ILogger<TokenService> logger)
    {
        _logger = logger;

        // JWT configuration values
        _key = configuration["Jwt:Key"] ?? throw new ArgumentNullException(nameof(_key), "JWT Key configuration is missing");
        _issuer = configuration["Jwt:Issuer"] ?? throw new ArgumentNullException(nameof(_issuer), "JWT Issuer configuration is missing");
        _audience = configuration["Jwt:Audience"] ?? throw new ArgumentNullException(nameof(_audience), "JWT Audience configuration is missing");
    }

    public string GenerateToken(int userId)
    {
        try
        {
            // Ensure _key is not null or empty
            if (string.IsNullOrEmpty(_key))
            {
                _logger.LogError("JWT Key is not configured.");
                throw new InvalidOperationException("JWT Key is not configured.");
            }

            // Convert the key to byte array
            var keyBytes = Encoding.UTF8.GetBytes(_key);

            // Create the JWT token
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.UserData, userId.ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                Issuer = _issuer,
                Audience = _audience,
              
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while generating JWT token.");
            throw;
        }
    }
}
