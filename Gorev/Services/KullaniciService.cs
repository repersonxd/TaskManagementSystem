using GorevY.Data;
using GorevY.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.IO;

namespace GorevY.Services
{
    public class KullaniciService
    {
        private readonly AppDbContext _context;
        private readonly string logFilePath = @"C:\Users\redpe\source\repos\Gorev\error_log.txt"; // Log dosyasının tam yolu

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

        public async Task<Kullanici> CreateKullanici(Kullanici kullanici)
        {
            try
            {
                _context.Kullanicilar.Add(kullanici);
                await _context.SaveChangesAsync();
                return kullanici;
            }
            catch (DbUpdateException ex)
            {
                var innerException = ex.InnerException;
                if (innerException != null)
                {
                    Console.WriteLine($"Hata Mesajı: {innerException.Message}");
                    Console.WriteLine($"Hata Türü: {innerException.GetType()}");
                    File.AppendAllText(logFilePath, $"Hata Mesajı: {innerException.Message}\nHata Türü: {innerException.GetType()}\n\n");
                }
                else
                {
                    Console.WriteLine($"Hata Mesajı: {ex.Message}");
                    Console.WriteLine($"Hata Türü: {ex.GetType()}");
                    File.AppendAllText(logFilePath, $"Hata Mesajı: {ex.Message}\nHata Türü: {ex.GetType()}\n\n");
                }
                throw; // Hatanın tekrar fırlatılması
            }
        }

        public async Task<Kullanici> UpdateKullanici(Kullanici kullanici)
        {
            try
            {
                _context.Entry(kullanici).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return kullanici;
            }
            catch (DbUpdateException ex)
            {
                var innerException = ex.InnerException;
                if (innerException != null)
                {
                    Console.WriteLine($"Hata Mesajı: {innerException.Message}");
                    Console.WriteLine($"Hata Türü: {innerException.GetType()}");
                    File.AppendAllText(logFilePath, $"Hata Mesajı: {innerException.Message}\nHata Türü: {innerException.GetType()}\n\n");
                }
                else
                {
                    Console.WriteLine($"Hata Mesajı: {ex.Message}");
                    Console.WriteLine($"Hata Türü: {ex.GetType()}");
                    File.AppendAllText(logFilePath, $"Hata Mesajı: {ex.Message}\nHata Türü: {ex.GetType()}\n\n");
                }
                throw; // Hatanın tekrar fırlatılması
            }
        }

        public async Task DeleteKullanici(int id)
        {
            try
            {
                var kullanici = await _context.Kullanicilar.FindAsync(id);
                if (kullanici != null)
                {
                    _context.Kullanicilar.Remove(kullanici);
                    await _context.SaveChangesAsync();
                }
            }
            catch (DbUpdateException ex)
            {
                var innerException = ex.InnerException;
                if (innerException != null)
                {
                    Console.WriteLine($"Hata Mesajı: {innerException.Message}");
                    Console.WriteLine($"Hata Türü: {innerException.GetType()}");
                    File.AppendAllText(logFilePath, $"Hata Mesajı: {innerException.Message}\nHata Türü: {innerException.GetType()}\n\n");
                }
                else
                {
                    Console.WriteLine($"Hata Mesajı: {ex.Message}");
                    Console.WriteLine($"Hata Türü: {ex.GetType()}");
                    File.AppendAllText(logFilePath, $"Hata Mesajı: {ex.Message}\nHata Türü: {ex.GetType()}\n\n");
                }
                throw; // Hatanın tekrar fırlatılması
            }
        }
    }
}
