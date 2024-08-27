using Microsoft.EntityFrameworkCore;
using GorevY.Models;

namespace GorevY.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Kullanici> Kullanicilar { get; set; } // Mevcut Kullanici DbSet
        public DbSet<Gorev> Tasks { get; set; } // Tasks DbSet
        public DbSet<RefreshToken> RefreshTokens { get; set; } // RefreshToken DbSet
    }
}
