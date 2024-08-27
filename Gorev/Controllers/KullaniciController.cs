using Microsoft.AspNetCore.Mvc;
using GorevY.Models;
using GorevY.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

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
        public async Task<ActionResult<List<Kullanici>>> GetKullanicilar()
        {
            var kullanicilar = await _kullaniciService.GetKullanicilar();
            if (kullanicilar == null || kullanicilar.Count == 0)
            {
                return NotFound("Kullanıcılar bulunamadı.");
            }
            return Ok(kullanicilar);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Kullanici>> GetKullanici(int id)
        {
            var kullanici = await _kullaniciService.GetKullaniciById(id);
            if (kullanici == null)
            {
                return NotFound($"Kullanıcı ID {id} bulunamadı.");
            }
            return Ok(kullanici);
        }

        [HttpPost]
        public async Task<ActionResult<Kullanici>> CreateKullanici([FromBody] Kullanici kullanici)
        {
            if (kullanici == null)
            {
                return BadRequest("Kullanıcı nesnesi boş.");
            }

            try
            {
                var createdKullanici = await _kullaniciService.CreateKullanici(kullanici);
                return CreatedAtAction(nameof(GetKullanici), new { id = createdKullanici.Id }, createdKullanici);
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, $"Sunucu hatası: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateKullanici(int id, [FromBody] Kullanici kullanici)
        {
            if (id != kullanici.Id)
            {
                return BadRequest("Kullanıcı ID'leri eşleşmiyor.");
            }

            var existingKullanici = await _kullaniciService.GetKullaniciById(id);
            if (existingKullanici == null)
            {
                return NotFound($"Kullanıcı ID {id} bulunamadı.");
            }

            try
            {
                await _kullaniciService.UpdateKullanici(kullanici);
                return NoContent();
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, $"Sunucu hatası: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteKullanici(int id)
        {
            var existingKullanici = await _kullaniciService.GetKullaniciById(id);
            if (existingKullanici == null)
            {
                return NotFound($"Kullanıcı ID {id} bulunamadı.");
            }

            try
            {
                await _kullaniciService.DeleteKullanici(id);
                return NoContent();
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, $"Sunucu hatası: {ex.Message}");
            }
        }
    }
}
