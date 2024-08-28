using Microsoft.AspNetCore.Mvc;
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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Kullanici>>> GetKullanicilar()
        {
            var kullanicilar = await _kullaniciService.GetKullanicilar();
            return new JsonResult(kullanicilar)
            {
                ContentType = "application/json; charset=utf-8"
            };
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Kullanici>> GetKullanici(int id)
        {
            var kullanici = await _kullaniciService.GetKullaniciById(id);

            if (kullanici == null)
            {
                return NotFound(new JsonResult(new { Message = "Kullanıcı bulunamadı." })
                {
                    ContentType = "application/json; charset=utf-8"
                });
            }

            return new JsonResult(kullanici)
            {
                ContentType = "application/json; charset=utf-8"
            };
        }

        [HttpPost]
        public async Task<ActionResult<Kullanici>> PostKullanici(Kullanici kullanici)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _kullaniciService.CreateKullanici(kullanici);

            return CreatedAtAction(nameof(GetKullanici), new { id = kullanici.Id }, new JsonResult(kullanici)
            {
                ContentType = "application/json; charset=utf-8"
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutKullanici(int id, Kullanici kullanici)
        {
            if (id != kullanici.Id)
            {
                return BadRequest(new JsonResult(new { Message = "ID uyuşmuyor." })
                {
                    ContentType = "application/json; charset=utf-8"
                });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingKullanici = await _kullaniciService.GetKullaniciById(id);
            if (existingKullanici == null)
            {
                return NotFound(new JsonResult(new { Message = "Kullanıcı bulunamadı." })
                {
                    ContentType = "application/json; charset=utf-8"
                });
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
                return NotFound(new JsonResult(new { Message = "Kullanıcı bulunamadı." })
                {
                    ContentType = "application/json; charset=utf-8"
                });
            }

            await _kullaniciService.DeleteKullanici(id);

            return NoContent();
        }
    }
}
