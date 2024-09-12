using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GorevY.DTOs;
using GorevY.Services;
using GorevY.Models;
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
            try
            {
                var kullanicilar = await _kullaniciService.GetKullanicilar();
                return Ok(kullanicilar);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving users: {ex.Message}");
                return StatusCode(500, new { Message = "Kullanıcılar alınırken bir hata oluştu. Lütfen tekrar deneyin.", Error = ex.Message });
            }
        }

        // Get user by ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Kullanici>> GetKullanici(int id)
        {
            try
            {
                var kullanici = await _kullaniciService.GetKullaniciById(id);
                if (kullanici == null)
                {
                    return NotFound(new { Message = "Kullanıcı bulunamadı." });
                }

                return Ok(kullanici);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Kullanıcı alınırken bir hata oluştu. Lütfen tekrar deneyin.", Error = ex.Message });
            }
        }

        // Register new user
        [HttpPost("Register")]
        public async Task<ActionResult<Kullanici>> Register(RegisterDto registerDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new { Message = "Geçersiz veri.", Errors = ModelState });
                }

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
                    Gorevler = new List<Gorev>()
                };

                await _kullaniciService.CreateKullanici(kullanici);
                return CreatedAtAction(nameof(GetKullanici), new { id = kullanici.Id }, kullanici);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Kullanıcı oluşturulurken bir hata oluştu. Lütfen tekrar deneyin.", Error = ex.Message });
            }
        }

        // Login user
        [HttpPost("Login")]
        public async Task<ActionResult> Login([FromBody] LoginDto loginDto)
        {
            try
            {
                var kullanici = await _kullaniciService.ValidateUser(loginDto.KullaniciAdi, loginDto.Sifre);
                if (kullanici == null)
                {
                    return Unauthorized(new { Message = "Geçersiz kullanıcı adı veya şifre." });
                }

                if (kullanici.Id == 0)
                {
                    return StatusCode(500, new { Message = "Kullanıcı ID alınamadı." });
                }

                var token = _kullaniciService.GenerateJwtToken(kullanici);

                return Ok(new
                {
                    Token = token,
                    KullaniciId = kullanici.Id
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Kullanıcı giriş yaparken bir hata oluştu. Lütfen tekrar deneyin.", Error = ex.Message });
            }
        }

        // Update user
        [HttpPut("{id}")]
        public async Task<IActionResult> PutKullanici(int id, Kullanici kullanici)
        {
            try
            {
                if (id != kullanici.Id)
                {
                    return BadRequest(new { Message = "ID uyuşmuyor." });
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(new { Message = "Geçersiz veri.", Errors = ModelState });
                }

                var existingKullanici = await _kullaniciService.GetKullaniciById(id);
                if (existingKullanici == null)
                {
                    return NotFound(new { Message = "Kullanıcı bulunamadı." });
                }

                await _kullaniciService.UpdateKullanici(kullanici);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Kullanıcı güncellenirken bir hata oluştu. Lütfen tekrar deneyin.", Error = ex.Message });
            }
        }

        // Delete user
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteKullanici(int id)
        {
            try
            {
                var kullanici = await _kullaniciService.GetKullaniciById(id);
                if (kullanici == null)
                {
                    return NotFound(new { Message = "Kullanıcı bulunamadı." });
                }

                await _kullaniciService.DeleteKullanici(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Kullanıcı silinirken bir hata oluştu. Lütfen tekrar deneyin.", Error = ex.Message });
            }
        }
    }
}
