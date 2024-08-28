using GorevY.Data;
using GorevY.Models;
using Microsoft.EntityFrameworkCore;

namespace GorevY.Services
{
    public class KullaniciService
    {
        private readonly AppDbContext _context;

        public KullaniciService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Kullanici>> GetKullanicilar()
        {
            return await _context.Kullanicilar.ToListAsync();
        }

        public async Task<Kullanici?> GetKullaniciById(int id)
        {
            return await _context.Kullanicilar.FindAsync(id);
        }

        public async Task CreateKullanici(Kullanici kullanici)
        {
            _context.Kullanicilar.Add(kullanici);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateKullanici(Kullanici kullanici)
        {
            _context.Entry(kullanici).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteKullanici(int id)
        {
            var kullanici = await GetKullaniciById(id);
            if (kullanici != null)
            {
                _context.Kullanicilar.Remove(kullanici);
                await _context.SaveChangesAsync();
            }
        }
    }
}
