using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace BabyHub.EntityFrameworkCore
{
    public class BabyHubDbContextFactory : IDesignTimeDbContextFactory<BabyHubDbContext>
    {
        public BabyHubDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<BabyHubDbContext>();

            // Жёстко задаём строку подключения
            optionsBuilder.UseSqlServer("Server=localhost;Database=BabyHub;Trusted_Connection=True;");

            return new BabyHubDbContext(optionsBuilder.Options);
        }

        //public BabyHubDbContext CreateDbContext(string[] args)
        //{
        //    var configuration = new ConfigurationBuilder()
        //        .SetBasePath(Directory.GetCurrentDirectory())
        //        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        //        .Build();

        //    var connectionString = configuration.GetConnectionString("Default");

        //    var optionsBuilder = new DbContextOptionsBuilder<BabyHubDbContext>();
        //    optionsBuilder.UseSqlServer(connectionString);

        //    return new BabyHubDbContext(optionsBuilder.Options);
        //}
    }
}
