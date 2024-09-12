using Microsoft.AspNetCore.Mvc;
using GorevY.Services;
using GorevY.Models;
using System.Security.Claims;
using GorevY.DTOs;
using Microsoft.Extensions.Logging;  // Loglama için

namespace GorevY.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController : ControllerBase
    {
        private readonly TaskService _taskService;
        private readonly KullaniciService _kullaniciService;  // Kullanıcı servisini ekledik
        private readonly ILogger<TasksController> _logger;  // Logger'ı ekledik

        public TasksController(TaskService taskService, KullaniciService kullaniciService, ILogger<TasksController> logger)
        {
            _taskService = taskService;
            _kullaniciService = kullaniciService; // Kullanıcı servisini yapılandırdık
            _logger = logger;  // Logger'ı yapılandırma
        }

        // Tüm görevleri getir
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Gorev>>> GetAllTasks()
        {
            try
            {
                var tasks = await _taskService.GetAllTasksAsync();
                return Ok(tasks);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Görevler alınırken bir hata oluştu.");
                return StatusCode(500, new { Message = "Görevler alınırken bir hata oluştu. Lütfen tekrar deneyin." });
            }
        }

        // ID ile görev getir
        [HttpGet("{id}")]
        public async Task<ActionResult<Gorev>> GetTask(int id)
        {
            try
            {
                var task = await _taskService.GetTaskById(id);
                if (task == null)
                {
                    _logger.LogWarning($"ID {id} ile görev bulunamadı.");
                    return NotFound(new { Message = "Görev bulunamadı." });
                }

                return Ok(task);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Görev ID {id} alınırken bir hata oluştu.");
                return StatusCode(500, new { Message = "Bir hata oluştu, lütfen tekrar deneyin." });
            }
        }

        // Görev ekle
        [HttpPost]
        public async Task<ActionResult<Gorev>> PostTask([FromBody] GorevDto gorevDto)
        {
            try
            {
                // DTO geçerliliğini kontrol et
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Geçersiz model gönderildi.");
                    return BadRequest(new { Message = "Geçersiz veri. Lütfen gerekli tüm alanları doldurun.", Errors = ModelState });
                }

                // Kullanıcı kimliğini JWT'den al
                var kullaniciIdClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

                if (kullaniciIdClaim == null)
                {
                    _logger.LogWarning("JWT'den kullanıcı kimliği alınamadı.");
                    return Unauthorized(new { Message = "Kullanıcı doğrulanamadı." });
                }

                if (!int.TryParse(kullaniciIdClaim.Value, out int kullaniciId))
                {
                    _logger.LogWarning("Kullanıcı kimliği geçersiz.");
                    return BadRequest(new { Message = "Geçersiz kullanıcı kimliği." });
                }

                var kullanici = await _kullaniciService.GetKullaniciById(kullaniciId);

                if (kullanici == null)
                {
                    _logger.LogWarning($"Kullanıcı ID {kullaniciId} bulunamadı.");
                    return BadRequest(new { Message = "Geçersiz kullanıcı." });
                }

                // Görev nesnesini oluştur
                var gorev = new Gorev
                {
                    GorevAdi = gorevDto.GorevAdi,
                    Aciklama = gorevDto.Aciklama,
                    Tamamlandi = gorevDto.Tamamlandi,
                    Kullanici = kullanici,  // Kullanıcıyı set ediyoruz
                    KullaniciId = kullanici.Id  // Kullanıcı ID'sini set ediyoruz
                };

                // Görevi kaydet
                await _taskService.CreateTask(gorev);
                _logger.LogInformation("Yeni görev başarıyla oluşturuldu.");

                return CreatedAtAction(nameof(GetTask), new { id = gorev.Id }, gorev);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Görev eklenirken bir hata oluştu.");
                return StatusCode(500, new { Message = "Görev eklenirken bir hata oluştu. Lütfen tekrar deneyin.", Error = ex.Message });
            }
        }
    }
}
