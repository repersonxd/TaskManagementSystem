using Microsoft.AspNetCore.Mvc;
using GorevY.DTOs;
using GorevY.Models;
using GorevY.Services;

namespace GorevY.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class KullaniciController : ControllerBase
    {
        private readonly KullaniciService _kullaniciService;

        public KullaniciController(KullaniciService kullaniciService)
        {
            _kullaniciService = kullaniciService;
        }

        // Get all users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Kullanici>>> GetKullanicilar()
        {
            var kullanicilar = await _kullaniciService.GetKullanicilar();
            return Ok(kullanicilar);
        }

        // Get user by ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Kullanici>> GetKullanici(int id)
        {
            var kullanici = await _kullaniciService.GetKullaniciById(id);
            if (kullanici == null)
            {
                return NotFound(new { Message = "Kullanıcı bulunamadı." });
            }

            return Ok(kullanici);
        }

        // Register new user
        [HttpPost("Register")]
        public async Task<ActionResult<Kullanici>> Register(RegisterDto registerDto)
        {
            if (registerDto.Sifre != registerDto.ConfirmSifre)
            {
                return BadRequest(new { Message = "Şifreler eşleşmiyor." });
            }

            var existingUser = await _kullaniciService.ValidateUser(registerDto.KullaniciAdi, registerDto.Sifre);
            if (existingUser != null)
            {
                return BadRequest(new { Message = "Bu kullanıcı adı zaten kullanılıyor." });
            }

            var kullanici = new Kullanici
            {
                KullaniciAdi = registerDto.KullaniciAdi,
                Email = registerDto.Email,
                Sifre = registerDto.Sifre,
                Gorevler = new List<Gorev>() // Ensure Gorevler is initialized
            };

            await _kullaniciService.CreateKullanici(kullanici);
            return CreatedAtAction(nameof(GetKullanici), new { id = kullanici.Id }, kullanici);
        }

        // Login user
        [HttpPost("Login")]
        public async Task<ActionResult> Login([FromBody] LoginDto loginDto)
        {
            var kullanici = await _kullaniciService.ValidateUser(loginDto.KullaniciAdi, loginDto.Sifre);
            if (kullanici == null)
            {
                return Unauthorized(new { Message = "Geçersiz kullanıcı adı veya şifre." });
            }

            var token = _kullaniciService.GenerateJwtToken(kullanici);
            return Ok(new
            {
                Token = token,
                KullaniciId = kullanici.Id
            });
        }

        // Update user
        [HttpPut("{id}")]
        public async Task<IActionResult> PutKullanici(int id, [FromBody] UserDto userDto)
        {
            if (id != userDto.Id)
            {
                return BadRequest(new { Message = "ID uyuşmuyor." });
            }

            var existingKullanici = await _kullaniciService.GetKullaniciById(id);
            if (existingKullanici == null)
            {
                return NotFound(new { Message = "Kullanıcı bulunamadı." });
            }

            existingKullanici.KullaniciAdi = userDto.KullaniciAdi;
            existingKullanici.Email = userDto.Email;
            existingKullanici.Sifre = userDto.Sifre;

            await _kullaniciService.UpdateKullanici(existingKullanici);
            return NoContent();
        }

        // Delete user
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteKullanici(int id)
        {
            var kullanici = await _kullaniciService.GetKullaniciById(id);
            if (kullanici == null)
            {
                return NotFound(new { Message = "Kullanıcı bulunamadı." });
            }

            await _kullaniciService.DeleteKullanici(id);
            return NoContent();
        }
    }
}
