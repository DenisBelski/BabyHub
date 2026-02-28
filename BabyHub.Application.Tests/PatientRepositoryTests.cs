using BabyHub.Domain.Patients;
using BabyHub.Domain.Shared.Enums;
using BabyHub.EntityFrameworkCore.Patients;
using FluentAssertions;
using NUnit.Framework;

namespace BabyHub.Application.Tests
{
    public class PatientRepositoryTests
    {
        [TestFixture]
        public class PatientRepository_DateSearch_Tests
        {
            [TestCase((int)EDateOperator.Equal, "2013-01-14", "Alice,Bob,Eve,EndOfDay")]
            [TestCase((int)EDateOperator.NotEqual, "2013-01-14", "Dave,Carol,Frank,Grace,Henry,Ivy,MidMarch")]
            [TestCase((int)EDateOperator.GreaterOrEqual, "2013-03-14", "Frank,Grace,Ivy,MidMarch")]
            [TestCase((int)EDateOperator.LessOrEqual, "2013-03-14", "Alice,Bob,Eve,Dave,Carol,Frank,Henry,EndOfDay,MidMarch")]
            [TestCase((int)EDateOperator.StartsAfter, "2013-03-14", "Grace,Ivy")]
            [TestCase((int)EDateOperator.EndsBefore, "2013-03-14", "Dave,Alice,Bob,Eve,Carol,Henry,EndOfDay")]
            [TestCase((int)EDateOperator.Approximate, "2013-03-14", "Frank,MidMarch")]
            public async Task GetListAsync_ShouldReturnCorrectPatients(
                int opValue,
                string dateStr,
                string expectedNamesCsv)
            {
                // Arrange
                using var context = TestDbContextFactory.Create();
                var repository = new PatientRepository(context);
                var op = (EDateOperator)opValue;
                var date = DateTime.Parse(dateStr);
                var expectedNames = expectedNamesCsv
                    .Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => x.Trim())
                    .OrderBy(x => x)
                    .ToArray();

                // Act
                var result = await repository.GetListAsync(date, op);
                var actualNames = result
                    .Select(x => x.FamilyName)
                    .OrderBy(x => x)
                    .ToArray();

                // Assert
                actualNames.Should().BeEquivalentTo(expectedNames);
            }

            [Test]
            public async Task GetListAsync_WithNullDate_ShouldReturnAllPatients()
            {
                // Arrange
                using var context = TestDbContextFactory.Create();
                var repository = new PatientRepository(context);

                // Act
                var result = await repository.GetListAsync(null, null);

                // Assert
                result.Should().HaveCount(11);
            }

            [Test]
            public async Task GetListAsync_WithNullOperator_ShouldReturnAllPatients()
            {
                // Arrange
                using var context = TestDbContextFactory.Create();
                var repository = new PatientRepository(context);

                // Act
                var result = await repository.GetListAsync(new DateTime(2013, 1, 14), null);

                // Assert
                result.Should().HaveCount(11);
            }

            [Test]
            public async Task GetListAsync_WithNullBirthDate_ShouldReturnAllPatients()
            {
                // Arrange
                using var context = TestDbContextFactory.Create();
                var repository = new PatientRepository(context);

                // Act
                var result = await repository.GetListAsync(null, EDateOperator.Equal);

                // Assert
                result.Should().HaveCount(11);
            }
        }
    }
}