using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using GorevY.Data;
using GorevY.Models;
using GorevY.DTOs;
using Microsoft.EntityFrameworkCore;

namespace GorevY.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly AppDbContext _context;

        public AuthController(IConfiguration configuration, AppDbContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            try
            {
                var kullanici = await _context.Kullanicilar
                    .FirstOrDefaultAsync(u => u.KullaniciAdi == loginDto.KullaniciAdi && u.Sifre == loginDto.Sifre);

                if (kullanici == null)
                {
                    return Unauthorized("Kullanıcı adı veya şifre hatalı.");
                }

                var key = _configuration["Jwt:Key"];
                if (string.IsNullOrEmpty(key) || key.Length < 32)
                {
                    return StatusCode(500, "JWT anahtarı eksik veya yetersiz uzunlukta.");
                }

                var tokenHandler = new JwtSecurityTokenHandler();
                var keyBytes = Encoding.ASCII.GetBytes(key);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[]
                    {
                        new Claim(ClaimTypes.Name, kullanici.Id.ToString())
                    }),
                    Expires = DateTime.UtcNow.AddHours(1),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                var tokenString = tokenHandler.WriteToken(token);

                // Refresh Token Oluştur
                var refreshToken = GenerateRefreshToken();
                var refreshTokenEntry = new RefreshToken
                {
                    Token = refreshToken,
                    Expiration = DateTime.UtcNow.AddDays(7),
                    UserId = kullanici.Id
                };
                await _context.RefreshTokens.AddAsync(refreshTokenEntry);
                await _context.SaveChangesAsync();

                return Ok(new { Token = tokenString, RefreshToken = refreshToken });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Sunucu hatası: {ex.Message}");
            }
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        // Diğer metotlar burada olacak...
    }
}
