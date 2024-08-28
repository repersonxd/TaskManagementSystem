using Microsoft.AspNetCore.Mvc;
using GorevY.Services;
using GorevY.Models;

namespace GorevY.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController : ControllerBase
    {
        private readonly TaskService _taskService;

        public TasksController(TaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Gorev>>> GetTasks()
        {
            var tasks = await _taskService.GetTasks();
            return Ok(tasks);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Gorev>> GetTask(int id)
        {
            var task = await _taskService.GetTaskById(id);

            if (task == null)
            {
                return NotFound(new { Message = "Görev bulunamadı." });
            }

            return Ok(task);
        }

        [HttpPost]
        public async Task<ActionResult<Gorev>> PostTask(Gorev task)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _taskService.CreateTask(task);

            return CreatedAtAction(nameof(GetTask), new { id = task.Id }, task);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutTask(int id, Gorev task)
        {
            if (id != task.Id)
            {
                return BadRequest(new { Message = "ID uyuşmuyor." });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingTask = await _taskService.GetTaskById(id);
            if (existingTask == null)
            {
                return NotFound(new { Message = "Görev bulunamadı." });
            }

            await _taskService.UpdateTask(task);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var task = await _taskService.GetTaskById(id);

            if (task == null)
            {
                return NotFound(new { Message = "Görev bulunamadı." });
            }

            await _taskService.DeleteTask(id);

            return NoContent();
        }
    }
}
