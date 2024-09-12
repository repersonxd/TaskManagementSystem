using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using GorevY.Data;
using GorevY.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GorevY.Services
{
    public class KullaniciService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration; // JWT için yapılandırma

        public KullaniciService(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // Tüm kullanıcıları getir
        public async Task<List<Kullanici>> GetKullanicilar()
        {
            return await _context.Kullanicilar.Include(k => k.Gorevler).ToListAsync();
        }

        // ID'ye göre kullanıcı getir
        public async Task<Kullanici?> GetKullaniciById(int id)
        {
            return await _context.Kullanicilar.Include(k => k.Gorevler).FirstOrDefaultAsync(k => k.Id == id);
        }

        // Yeni kullanıcı oluştur
        public async Task CreateKullanici(Kullanici kullanici)
        {
            _context.Kullanicilar.Add(kullanici);
            await _context.SaveChangesAsync();
        }

        // Kullanıcı güncelle
        public async Task UpdateKullanici(Kullanici kullanici)
        {
            _context.Kullanicilar.Update(kullanici);
            await _context.SaveChangesAsync();
        }

        // Kullanıcı sil
        public async Task DeleteKullanici(int id)
        {
            var kullanici = await _context.Kullanicilar.FindAsync(id);
            if (kullanici != null)
            {
                _context.Kullanicilar.Remove(kullanici);
                await _context.SaveChangesAsync();
            }
        }

        // Kullanıcı doğrulama
        public async Task<Kullanici?> ValidateUser(string kullaniciAdi, string sifre)
        {
            return await _context.Kullanicilar
                .FirstOrDefaultAsync(k => k.KullaniciAdi == kullaniciAdi && k.Sifre == sifre);
        }

        // JWT token oluşturma
        public string GenerateJwtToken(Kullanici kullanici)
        {
            var key = _configuration["Jwt:Key"];
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key), "JWT Key yapılandırma ayarı eksik");
            }

            var keyBytes = Encoding.UTF8.GetBytes(key);
            var creds = new SigningCredentials(new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, kullanici.KullaniciAdi),
                new Claim("userId", kullanici.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(60),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
