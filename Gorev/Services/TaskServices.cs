using GorevY.Data;
using GorevY.Models;
using Microsoft.EntityFrameworkCore;

namespace GorevY.Services
{
    public class TaskService
    {
        private readonly AppDbContext _context;

        public TaskService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Gorev>> GetTasks()
        {
            return await _context.Gorevler.ToListAsync();
        }

        public async Task<Gorev?> GetTaskById(int id)
        {
            return await _context.Gorevler.FindAsync(id);
        }

        public async Task CreateTask(Gorev task)
        {
            _context.Gorevler.Add(task);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateTask(Gorev task)
        {
            _context.Entry(task).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteTask(int id)
        {
            var task = await GetTaskById(id);
            if (task != null)
            {
                _context.Gorevler.Remove(task);
                await _context.SaveChangesAsync();
            }
        }
    }
}
