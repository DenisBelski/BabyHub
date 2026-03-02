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
            [TestCase((int)EDateOperator.Equal, "2013-01-14", "Alice, Bob, Eve, EndOfDay")] // date-only: matches entire day
            [TestCase((int)EDateOperator.Equal, "2013-01-14T10:00:00", "Bob")] // exact time: matches only that instant
            [TestCase((int)EDateOperator.Equal, "2013-01-16", "")] // no patients on this day

            [TestCase((int)EDateOperator.NotEqual, "2013-01-14", "Dave, Carol, CarolMid, Frank, Grace, Henry, Ivy, MidMarch, EndMarch")] // date-only: excludes entire day
            [TestCase((int)EDateOperator.NotEqual, "2013-01-14T10:00:00", "Alice, Eve, EndOfDay, Dave, Carol, CarolMid, Frank, Grace, Henry, Ivy, MidMarch, EndMarch")] // exact time: excludes only that instant

            [TestCase((int)EDateOperator.LessThan, "2013-01-14", "Dave")] // date-only: everything before start of day
            [TestCase((int)EDateOperator.LessThan, "2013-01-14T10:00:00", "Dave, Alice")] // exact time: everything strictly before that instant

            [TestCase((int)EDateOperator.NotEqual, "2013-01-14", "Dave, Carol, CarolMid, Frank, Grace, Henry, Ivy, MidMarch, EndMarch")] // date-only: excludes entire day
            [TestCase((int)EDateOperator.GreaterThan, "2013-01-14T10:00:00", "Eve, EndOfDay, Carol, CarolMid, Frank, Grace, Henry, Ivy, MidMarch, EndMarch")] // exact time: everything strictly after that instant

            [TestCase((int)EDateOperator.LessOrEqual, "2013-01-14", "Dave, Alice, Bob, Eve, EndOfDay")] // date-only: everything up to and including entire day
            [TestCase((int)EDateOperator.LessOrEqual, "2013-01-14T10:00:00", "Dave, Alice, Bob")] // exact time: everything up to and including that instant
            [TestCase((int)EDateOperator.LessOrEqual, "2013-03-14", "Dave, Alice, Bob, Eve, EndOfDay, Carol, CarolMid, Henry, Frank, MidMarch, EndMarch")] // includes entire target day

            [TestCase((int)EDateOperator.GreaterOrEqual, "2013-03-14", "Frank, MidMarch, EndMarch, Grace, Ivy")] // includes entire target day
            [TestCase((int)EDateOperator.GreaterOrEqual, "2013-01-14T10:00:00", "Bob, Eve, EndOfDay, Carol, CarolMid, Frank, Grace, Henry, Ivy, MidMarch, EndMarch")] // exact time: inclusive

            [TestCase((int)EDateOperator.StartsAfter, "2013-01-14", "Carol, CarolMid, Frank, Grace, Henry, Ivy, MidMarch, EndMarch")] // strictly after end of that day
            [TestCase((int)EDateOperator.StartsAfter, "2013-03-14", "Grace, Ivy")] // strictly after end of Mar 14

            [TestCase((int)EDateOperator.EndsBefore, "2013-01-14", "Dave")] // strictly before start of that day
            [TestCase((int)EDateOperator.EndsBefore, "2013-03-14", "Dave, Alice, Bob, Eve, EndOfDay, Carol, CarolMid, Henry")] // strictly before start of Mar 14

            [TestCase((int)EDateOperator.Approximate, "2013-01-14", "Alice, Bob, Eve, EndOfDay")] // behaves like eq for date-only
            [TestCase((int)EDateOperator.Approximate, "2013-03-14", "Frank, MidMarch, EndMarch")] // behaves like eq for date-only
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
                using var context = TestDbContextFactory.Create();
                var repository = new PatientRepository(context);

                var result = await repository.GetListAsync(null, null);

                result.Should().HaveCount(13);
            }

            [Test]
            public async Task GetListAsync_WithNullOperator_ShouldReturnAllPatients()
            {
                using var context = TestDbContextFactory.Create();
                var repository = new PatientRepository(context);

                var result = await repository.GetListAsync(new DateTime(2013, 1, 14), null);

                result.Should().HaveCount(13);
            }

            [Test]
            public async Task GetListAsync_WithNullBirthDate_ShouldReturnAllPatients()
            {
                using var context = TestDbContextFactory.Create();
                var repository = new PatientRepository(context);

                var result = await repository.GetListAsync(null, EDateOperator.Equal);

                result.Should().HaveCount(13);
            }

            [Test]
            public async Task GetListAsync_WithCount_ShouldLimitResults()
            {
                using var context = TestDbContextFactory.Create();
                var repository = new PatientRepository(context);

                var result = await repository.GetListAsync(null, null, count: 5);

                result.Should().HaveCount(5);
            }

            [Test]
            public async Task GetListAsync_WithCountAndFilter_ShouldLimitFilteredResults()
            {
                using var context = TestDbContextFactory.Create();
                var repository = new PatientRepository(context);

                // eq2013-01-14 returns 4 patients, limit to 2
                var result = await repository.GetListAsync(
                    new DateTime(2013, 1, 14), EDateOperator.Equal, count: 2);

                result.Should().HaveCount(2);
            }
        }
    }
}