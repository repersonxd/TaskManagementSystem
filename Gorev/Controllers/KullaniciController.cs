using Microsoft.AspNetCore.Mvc;
using GorevY.Services;
using GorevY.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace GorevY.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class KullaniciController : ControllerBase
    {
        private readonly KullaniciService _kullaniciService;
        private readonly ILogger<KullaniciController> _logger;

        public KullaniciController(KullaniciService kullaniciService, ILogger<KullaniciController> logger)
        {
            _kullaniciService = kullaniciService;
            _logger = logger;
        }

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
                _logger.LogError(ex, "Kullanıcıları alırken bir hata oluştu.");
                return StatusCode(500, "Kullanıcıları alırken bir hata oluştu.");
            }
        }

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
                _logger.LogError(ex, "Kullanıcıyı alırken bir hata oluştu.");
                return StatusCode(500, "Kullanıcıyı alırken bir hata oluştu.");
            }
        }

        [HttpPost]
        public async Task<ActionResult<Kullanici>> PostKullanici(Kullanici kullanici)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _kullaniciService.CreateKullanici(kullanici);
                return CreatedAtAction(nameof(GetKullanici), new { id = kullanici.Id }, kullanici);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kullanıcı oluşturulurken bir hata oluştu.");
                return StatusCode(500, "Kullanıcı oluşturulurken bir hata oluştu.");
            }
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
                return BadRequest(ModelState);
            }

            try
            {
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
                _logger.LogError(ex, "Kullanıcı güncellenirken bir hata oluştu.");
                return StatusCode(500, "Kullanıcı güncellenirken bir hata oluştu.");
            }
        }

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
                _logger.LogError(ex, "Kullanıcı silinirken bir hata oluştu.");
                return StatusCode(500, "Kullanıcı silinirken bir hata oluştu.");
            }
        }
    }
}
