using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace BabyHub.EntityFrameworkCore
{
    public class BabyHubDbContextFactory : IDesignTimeDbContextFactory<BabyHubDbContext>
    {
        public BabyHubDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<BabyHubDbContext>();
            optionsBuilder.UseSqlServer("Server=localhost;Database=BabyHub;Trusted_Connection=True;");

            return new BabyHubDbContext(optionsBuilder.Options);
        }
    }
}
