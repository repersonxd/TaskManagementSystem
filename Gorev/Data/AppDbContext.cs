using GorevY.Models;
using Microsoft.EntityFrameworkCore;

namespace GorevY.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Kullanici> Kullanicilar { get; set; }
        public DbSet<Gorev> Gorevler { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
    }
}
