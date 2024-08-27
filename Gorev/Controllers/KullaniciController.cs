using Microsoft.AspNetCore.Mvc;
using GorevY.Services;
using GorevY.Models;
using System.Collections.Generic;
using System.Linq;
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

        [HttpPost]
        public async Task<ActionResult<Kullanici>> PostKullanici(Kullanici kullanici)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _kullaniciService.CreateKullanici(kullanici);

            return CreatedAtAction(nameof(GetKullanici), new { id = kullanici.Id }, kullanici);
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
