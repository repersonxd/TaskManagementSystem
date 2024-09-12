using GorevY.Data;
using GorevY.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GorevY.Services
{
    public class TaskService
    {
        private readonly AppDbContext _context;
        private readonly ILogger<TaskService> _logger;

        public TaskService(AppDbContext context, ILogger<TaskService> logger)
        {
            _context = context;
            _logger = logger;
        }

        // Get all tasks
        public async Task<List<Gorev>> GetAllTasksAsync()
        {
            try
            {
                return await _context.Gorevler.ToListAsync();
            }
            catch (Exception ex)
            {
                // Log the error
                _logger.LogError(ex, "Error occurred while fetching tasks.");
                throw new Exception("Görevler alınırken bir hata oluştu.", ex);
            }
        }

        // Get task by ID
        public async Task<Gorev?> GetTaskById(int id)
        {
            try
            {
                return await _context.Gorevler.FirstOrDefaultAsync(g => g.Id == id);
            }
            catch (Exception ex)
            {
                // Log the error
                _logger.LogError(ex, $"Error occurred while fetching task with ID {id}.");
                throw new Exception($"Görev ID {id} alınırken bir hata oluştu.", ex);
            }
        }

        // Create new task
        public async Task CreateTask(Gorev task)
        {
            try
            {
                _context.Gorevler.Add(task);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Log the error
                _logger.LogError(ex, "Error occurred while creating a new task.");
                throw new Exception("Görev eklenirken bir hata oluştu.", ex);
            }
        }

        // Update task
        public async Task UpdateTask(Gorev task)
        {
            try
            {
                _context.Gorevler.Update(task);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Log the error
                _logger.LogError(ex, "Error occurred while updating the task.");
                throw new Exception("Görev güncellenirken bir hata oluştu.", ex);
            }
        }

        // Delete task
        public async Task DeleteTask(int id)
        {
            try
            {
                var task = await _context.Gorevler.FindAsync(id);
                if (task != null)
                {
                    _context.Gorevler.Remove(task);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                // Log the error
                _logger.LogError(ex, $"Error occurred while deleting the task with ID {id}.");
                throw new Exception($"Görev ID {id} silinirken bir hata oluştu.", ex);
            }
        }
    }
}
