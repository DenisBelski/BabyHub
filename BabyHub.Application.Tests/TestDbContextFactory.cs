using BabyHub.Domain.Patients;
using BabyHub.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BabyHub.Application.Tests
{
    public static class TestDbContextFactory
    {
        public static BabyHubDbContext Create()
        {
            var options = new DbContextOptionsBuilder<BabyHubDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var context = new BabyHubDbContext(options);

            SeedData(context);

            return context;
        }

        private static void SeedData(BabyHubDbContext context)
        {
            context.Patients.AddRange(
                new Patient("Alice", new DateTime(2013, 1, 14, 0, 0, 0)),
                new Patient("Bob", new DateTime(2013, 1, 14, 10, 0, 0)),
                new Patient("Eve", new DateTime(2013, 1, 14, 12, 0, 0)),
                new Patient("Dave", new DateTime(2013, 1, 13, 12, 0, 0)),
                new Patient("Carol", new DateTime(2013, 1, 15, 0, 0, 0)),
                new Patient("Frank", new DateTime(2013, 3, 14, 0, 0, 0)),
                new Patient("Henry", new DateTime(2013, 1, 21, 0, 0, 0)),
                new Patient("Grace", new DateTime(2013, 3, 15, 0, 0, 0)),
                new Patient("Ivy", new DateTime(2015, 6, 15, 0, 0, 0)),
                new Patient("EndOfDay", new DateTime(2013, 1, 14, 23, 59, 59)),
                new Patient("MidMarch", new DateTime(2013, 3, 14, 15, 0, 0))
            );

            context.SaveChanges();
        }
    }
}