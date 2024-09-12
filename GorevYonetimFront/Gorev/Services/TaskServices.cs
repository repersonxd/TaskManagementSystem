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

        // Tüm görevleri getir
        public async Task<List<Gorev>> GetAllTasksAsync()
        {
            try
            {
                return await _context.Gorevler.ToListAsync();
            }
            catch (Exception ex)
            {
                // Hata loglama
                throw new Exception("Görevler alınırken bir hata oluştu.", ex);
            }
        }

        // ID ile görevi getir
        public async Task<Gorev?> GetTaskById(int id)
        {
            try
            {
                return await _context.Gorevler.FirstOrDefaultAsync(g => g.Id == id);
            }
            catch (Exception ex)
            {
                // Hata loglama
                throw new Exception($"Görev ID {id} alınırken bir hata oluştu.", ex);
            }
        }

        // Yeni görev oluştur
        public async Task CreateTask(Gorev task)
        {
            try
            {
                _context.Gorevler.Add(task);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Hata loglama
                throw new Exception("Görev eklenirken bir hata oluştu.", ex);
            }
        }

        // Görevi güncelle
        public async Task UpdateTask(Gorev task)
        {
            try
            {
                if (_context.Gorevler.Any(g => g.Id == task.Id))
                {
                    _context.Entry(task).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                }
                else
                {
                    throw new Exception("Güncellenecek görev bulunamadı.");
                }
            }
            catch (Exception ex)
            {
                // Hata loglama
                throw new Exception($"Görev güncellenirken bir hata oluştu. ID: {task.Id}", ex);
            }
        }

        // Görevi sil
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
                else
                {
                    throw new Exception("Silinecek görev bulunamadı.");
                }
            }
            catch (Exception ex)
            {
                // Hata loglama
                throw new Exception($"Görev silinirken bir hata oluştu. ID: {id}", ex);
            }
        }
    }
}
