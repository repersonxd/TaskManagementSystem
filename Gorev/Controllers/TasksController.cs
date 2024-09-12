using Microsoft.AspNetCore.Mvc;
using GorevY.Services;
using GorevY.Models;
using System.Security.Claims;
using GorevY.DTOs;
using Microsoft.Extensions.Logging;

namespace GorevY.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController : ControllerBase
    {
        private readonly TaskService _taskService;
        private readonly KullaniciService _kullaniciService;
        private readonly ILogger<TasksController> _logger;

        public TasksController(TaskService taskService, KullaniciService kullaniciService, ILogger<TasksController> logger)
        {
            _taskService = taskService;
            _kullaniciService = kullaniciService;
            _logger = logger;
        }

        // Get all tasks
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
                _logger.LogError(ex, "Error retrieving tasks.");
                return StatusCode(500, new { Message = "An error occurred while retrieving tasks. Please try again." });
            }
        }

        // Get task by ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Gorev>> GetTask(int id)
        {
            try
            {
                var task = await _taskService.GetTaskById(id);
                if (task == null)
                {
                    _logger.LogWarning($"Task with ID {id} not found.");
                    return NotFound(new { Message = "Task not found." });
                }

                return Ok(task);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving task with ID {id}.");
                return StatusCode(500, new { Message = "An error occurred while retrieving the task. Please try again." });
            }
        }

        // Create task
        [HttpPost]
        public async Task<ActionResult<Gorev>> PostTask([FromBody] GorevDto gorevDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid model submitted.");
                    return BadRequest(new { Message = "Invalid data. Please fill in all required fields.", Errors = ModelState });
                }

                var kullaniciIdClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.UserData);

                if (kullaniciIdClaim == null)
                {
                    _logger.LogWarning("User ID could not be retrieved from JWT.");
                    return Unauthorized(new { Message = "User not authenticated." });
                }

                if (!int.TryParse(kullaniciIdClaim.Value, out int kullaniciId))
                {
                    _logger.LogWarning("Invalid user ID.");
                    return BadRequest(new { Message = "Invalid user ID." });
                }

                var kullanici = await _kullaniciService.GetKullaniciById(kullaniciId);

                if (kullanici == null)
                {
                    _logger.LogWarning($"User with ID {kullaniciId} not found.");
                    return BadRequest(new { Message = "Invalid user." });
                }

                var gorev = new Gorev
                {
                    GorevAdi = gorevDto.GorevAdi,
                    Aciklama = gorevDto.Aciklama,
                    Tamamlandi = gorevDto.Tamamlandi,
                    Kullanici = kullanici,
                    KullaniciId = kullanici.Id
                };

                await _taskService.CreateTask(gorev);
                _logger.LogInformation("New task successfully created.");

                return CreatedAtAction(nameof(GetTask), new { id = gorev.Id }, gorev);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating task.");
                return StatusCode(500, new { Message = "An error occurred while creating the task. Please try again.", Error = ex.Message });
            }
        }
    }
}
