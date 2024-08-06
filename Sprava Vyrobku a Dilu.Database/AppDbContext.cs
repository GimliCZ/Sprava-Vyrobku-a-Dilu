using Microsoft.EntityFrameworkCore;
using Sprava_Vyrobku_a_Dilu.Database.Models;

namespace Sprava_Vyrobku_a_Dilu.Database
{
    public class AppDbContext : DbContext
    {
        public DbSet<DilModel> Dily {  get; set; }
        public DbSet<VyrobekModel> Vyrobky {  get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DilModel>()
                .ToTable("DilModel")
                .HasOne(d => d.Vyrobek)
                .WithMany(v => v.Dily)
                .HasForeignKey(d => d.VyrobekId)
                .IsRequired();

            modelBuilder.Entity<VyrobekModel>()
                .ToTable("VyrobekModel");

            modelBuilder.Entity<DilModel>()
                .Property(d => d.Cena)
                .HasPrecision(18, 2); // Precision: 18, Scale: 2

            modelBuilder.Entity<VyrobekModel>()
                .Property(v => v.Cena)
                .HasPrecision(18, 2); // Precision: 18, Scale: 2

            base.OnModelCreating(modelBuilder);
        }
    }
}
