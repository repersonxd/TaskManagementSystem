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

        // OnModelCreating metodu, gerekli tablo adları, ilişkiler ve diğer model yapılandırmaları için kullanılabilir
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Eğer tablo adlarını özel olarak belirlemek isterseniz:
            modelBuilder.Entity<Kullanici>().ToTable("Kullanicilar");
            modelBuilder.Entity<Gorev>().ToTable("Gorevler");
            modelBuilder.Entity<RefreshToken>().ToTable("RefreshTokens");
        }
    }
}
