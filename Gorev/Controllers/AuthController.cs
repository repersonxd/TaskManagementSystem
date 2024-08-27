using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using GorevY.Data;
using GorevY.Models;
using GorevY.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
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
                    .SingleOrDefaultAsync(u => u.KullaniciAdi == loginDto.KullaniciAdi && u.Sifre == loginDto.Sifre);

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

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] TokenRequest request)
        {
            try
            {
                if (request == null || string.IsNullOrEmpty(request.Token) || string.IsNullOrEmpty(request.RefreshToken))
                {
                    return BadRequest("Geçersiz istek.");
                }

                var principal = GetPrincipalFromExpiredToken(request.Token);
                var userIdString = principal.Identity?.Name; // null kontrolü
                if (string.IsNullOrEmpty(userIdString))
                {
                    return BadRequest("Token'dan kullanıcı bilgisi alınamadı.");
                }

                if (!int.TryParse(userIdString, out var userId))
                {
                    return BadRequest("Geçersiz kullanıcı ID.");
                }

                var user = await _context.Kullanicilar.FindAsync(userId);
                if (user == null || !await _context.RefreshTokens.AnyAsync(rt => rt.Token == request.RefreshToken && rt.UserId == userId && !rt.IsRevoked))
                {
                    return BadRequest("Geçersiz token.");
                }

                var newJwtToken = GenerateJwtToken(principal.Claims);
                var newRefreshToken = GenerateRefreshToken();

                var refreshToken = await _context.RefreshTokens.FirstOrDefaultAsync(rt => rt.Token == request.RefreshToken);
                if (refreshToken == null)
                {
                    return BadRequest("Geçersiz refresh token.");
                }
                refreshToken.Token = newRefreshToken;
                refreshToken.Expiration = DateTime.UtcNow.AddDays(7);

                await _context.SaveChangesAsync();

                return Ok(new { Token = newJwtToken, RefreshToken = newRefreshToken });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Sunucu hatası: {ex.Message}");
            }
        }

        private string GenerateJwtToken(IEnumerable<Claim> claims)
        {
            var key = _configuration["Jwt:Key"];
            if (string.IsNullOrEmpty(key) || key.Length < 32)
            {
                throw new InvalidOperationException("JWT anahtarı eksik veya yetersiz uzunlukta.");
            }

            var keyBytes = Encoding.UTF8.GetBytes(key);
            var creds = new SigningCredentials(new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
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

        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var key = _configuration["Jwt:Key"];
            if (string.IsNullOrEmpty(key) || key.Length < 32)
            {
                throw new InvalidOperationException("JWT anahtarı eksik veya yetersiz uzunlukta.");
            }

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                ValidateLifetime = false // Süresi dolmuş token'ları geçerli kılmak için
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);

            if (!(securityToken is JwtSecurityToken jwtSecurityToken) || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Geçersiz token.");
            }

            return principal;
        }
    }
}
