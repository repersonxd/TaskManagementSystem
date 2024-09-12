using Microsoft.AspNetCore.Mvc;
using GorevY.Services;
using GorevY.Models;
using GorevY.DTOs;

namespace GorevY.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly KullaniciService _kullaniciService;

        public AuthController(KullaniciService kullaniciService)
        {
            _kullaniciService = kullaniciService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            if (registerDto.Sifre != registerDto.ConfirmSifre)
            {
                return BadRequest("Þifreler eþleþmiyor.");
            }

            var kullanici = new Kullanici
            {
                KullaniciAdi = registerDto.KullaniciAdi,
                Email = registerDto.Email,
                Sifre = registerDto.Sifre,
                Gorevler = new List<Gorev>()  // Boþ bir görev listesi ekleniyor
            };

            await _kullaniciService.CreateKullanici(kullanici);

            return Ok("Kayýt baþarýlý.");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var kullanici = await _kullaniciService.ValidateUser(loginDto.KullaniciAdi, loginDto.Sifre);
            if (kullanici == null)
            {
                return Unauthorized("Geçersiz kullanýcý adý veya þifre.");
            }

            var token = _kullaniciService.GenerateJwtToken(kullanici);
            return Ok(new { Token = token });
        }
    }
}
