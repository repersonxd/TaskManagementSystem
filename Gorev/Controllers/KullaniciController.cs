    using Microsoft.AspNetCore.Mvc;
    using GorevY.Services;
    using GorevY.Models;
    using GorevY.DTOs;

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

            [HttpGet]
            public async Task<ActionResult<IEnumerable<Kullanici>>> GetKullanicilar()
            {
                var kullanicilar = await _kullaniciService.GetKullanicilar();
                return Ok(kullanicilar);
            }

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

            [HttpPost("Register")]
            public async Task<ActionResult<Kullanici>> Register(RegisterDto registerDto)
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new { Message = "Geçersiz veri.", Errors = ModelState });
                }

                if (registerDto.Sifre != registerDto.ConfirmSifre)
                {
                    return BadRequest(new { Message = "Şifreler eşleşmiyor." });
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

            [HttpPost("Login")]
            public async Task<ActionResult> Login([FromBody] LoginDto loginDto)
            {
                var kullanici = await _kullaniciService.ValidateUser(loginDto.KullaniciAdi, loginDto.Sifre);
                if (kullanici == null)
                {
                    return Unauthorized(new { Message = "Geçersiz kullanıcı adı veya şifre." });
                }

                var token = _kullaniciService.GenerateJwtToken(kullanici);
                return Ok(new { Token = token });
            }

            [HttpPut("{id}")]
            public async Task<IActionResult> PutKullanici(int id, Kullanici kullanici)
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
