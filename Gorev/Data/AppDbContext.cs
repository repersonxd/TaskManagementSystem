using Microsoft.EntityFrameworkCore;
using GorevY.Models;

namespace GorevY.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<Kullanici> Kullanicilar { get; set; }
        public DbSet<Gorev> Gorevler { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
    }

}
