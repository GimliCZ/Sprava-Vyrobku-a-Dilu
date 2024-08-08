using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace SpravaVyrobkuaDilu.Database
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

            // Set up the connection string
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=EFCoreDatabase;Trusted_Connection=True;");

            return new AppDbContext(optionsBuilder.Options);
        }
    }
}
