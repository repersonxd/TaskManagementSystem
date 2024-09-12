using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public class TokenService
{
    private readonly string _key;
    private readonly string _issuer;
    private readonly string _audience;

    public TokenService(IConfiguration configuration)
    {
        // JWT yapýlandýrma deðerlerini al ve null kontrolü yap
        _key = configuration["Jwt:Key"] ?? throw new ArgumentNullException(nameof(_key), "JWT Key configuration is missing");
        _issuer = configuration["Jwt:Issuer"] ?? throw new ArgumentNullException(nameof(_issuer), "JWT Issuer configuration is missing");
        _audience = configuration["Jwt:Audience"] ?? throw new ArgumentNullException(nameof(_audience), "JWT Audience configuration is missing");
    }

    public string GenerateToken(int userId)
    {
        // Eðer _key boþ ya da null ise, hata fýrlat
        if (string.IsNullOrEmpty(_key))
        {
            throw new InvalidOperationException("JWT Key is not configured.");
        }

        // Anahtarý byte dizisine dönüþtür
        var keyBytes = Encoding.UTF8.GetBytes(_key);

        // JWT Token oluþturucu
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                // Token'a kullanýcý ID'sini claim olarak ekle
                new Claim(ClaimTypes.NameIdentifier, userId.ToString())
            }),
            // Token geçerlilik süresi (1 saat)
            Expires = DateTime.UtcNow.AddHours(1),
            Issuer = _issuer,  // Yapýlandýrma dosyasýndaki Issuer deðeri
            Audience = _audience,  // Yapýlandýrma dosyasýndaki Audience deðeri
            // Anahtar ile token'ý imzala
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha256Signature)
        };

        // Token oluþtur
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);  // Token'ý string olarak geri döndür
    }
}
