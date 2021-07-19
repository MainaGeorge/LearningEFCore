using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Linq;

namespace EFCore5WebApp.DAL.Tests
{
    [TestFixture]
    public class AggregationTests
    {
        private AppDbContext _context;

        [SetUp]
        public void SetUp()
        {
            _context = new AppDbContext(
                new DbContextOptionsBuilder<AppDbContext>()
                    .UseSqlServer(
                        "Server = (localdb)\\mssqllocaldb; Database = EfCoreLearningWebApp;Trusted_Connection=True;MultipleActiveResultSets=true")
                    .Options
            );
        }

        [Test]
        public void CountPersons()
        {
            var people = _context.Persons.Count();

            Assert.AreEqual(2, people);
        }

        [Test]
        public void CountPersonWithFilter()
        {
            var person = _context.Persons
                .Count(p => p.FirstName.Equals("joHn")
                            && p.LastName.Equals("smIth"));

            Assert.AreEqual(1, person);
        }

        [Test]
        public void GetMinAge()
        {
            var minAge = _context.Persons.Min(x => x.Age);
            Assert.AreEqual(26, minAge);
        }

        [Test]
        public void GetMaxAge()
        {
            var maxAge = _context.Persons.Max(x => x.Age);
            Assert.AreEqual(30, maxAge);
        }

        [Test]
        public void GetAverageAge()
        {
            var averageAge = _context.Persons.Average(x => x.Age);
            Assert.AreEqual(28, averageAge);
        }

        [Test]
        public void GetTotalAgeSum()
        {
            var ageSum = _context.Persons.Sum(x => x.Age);
            Assert.AreEqual(56, ageSum);
        }

        [Test]
        public void GroupAddressesByStateCount()
        {
            var expectedAddressesFromILCount = _context.Addresses.Count(a => a.State == "IL");
            var expectedAddressesFromCACount = _context.Addresses.Count(a => a.State == "CA");

            var groupedAddresses = (from a in _context.Addresses.ToList()
                    group a by a.State
                    into g
                    select new
                    {
                        State = g.Key,
                        Count = g.Count()
                    })
                .ToList();

            Assert.AreEqual(expectedAddressesFromILCount, groupedAddresses.Single(x =>
                x.State == "IL").Count);
            Assert.AreEqual(expectedAddressesFromCACount, groupedAddresses.Single(x =>
                x.State == "CA").Count);

        }

        [Test]
        public void MinAgePerState()
        {
            const int expectedMinAgeInCA = 30;
            const int expectedMinAgeInIL = 26;

            var groupingByState = _context.Addresses
                .Select(x => new {x.State, x.Person.Age})
                .GroupBy(x => x.State)
                .Select(a => new
                {
                    State = a.Key,
                    MinAge = a.Min(pa => pa.Age)
                });
                

            Assert.AreEqual(expectedMinAgeInCA, groupingByState.Single(s => s.State == "CA").MinAge);
            Assert.AreEqual(expectedMinAgeInIL, groupingByState.Single(s => s.State == "IL").MinAge);
        }

        [Test]
        public void MaxAgePerState()
        {
            const int expectedMaxAgeIL = 26;
            const int expectedMaxAgeCA = 30;

            var groupingByState = _context.Addresses
                .Select(a => new {a.State, a.Person.Age})
                .GroupBy(g => g.State)
                .Select(s => new
                {
                    State = s.Key,
                    MaxAge = s.Max(e => e.Age)
                });

            Assert.AreEqual(expectedMaxAgeCA, groupingByState.Single(s => s.State == "CA").MaxAge);
            Assert.AreEqual(expectedMaxAgeIL, groupingByState.Single(s => s.State == "IL").MaxAge);
        }

        [Test]
        public void SumAgePerState()
        {
            const int expectedSumAgeIL = 52;
            const int expectedSumAgeCA = 30;

            var groupingByState = _context.Addresses
                .Select(a => new {a.Person.Age, a.State})
                .GroupBy(a => a.State)
                .Select(a => new
                {
                    AgeTotal = a.Sum(e => e.Age),
                    State = a.Key
                });

            Assert.AreEqual(expectedSumAgeCA, groupingByState.Single(x => x.State == "CA").AgeTotal);
            Assert.AreEqual(expectedSumAgeIL, groupingByState.Single(x => x.State == "IL").AgeTotal);
        }

        [Test]
        public void AverageAgePerState()
        {
            const int expectedAverageAgeIL = 26;
            const int expectedAverageAgeCA = 30;

            var groupingByState = _context.Addresses
                .Select(a => new {a.State, a.Person.Age})
                .GroupBy(a => a.State)
                .Select(e => new
                {
                    State = e.Key,
                    AverageAge = e.Average(a => a.Age)
                });

            Assert.AreEqual(expectedAverageAgeCA, groupingByState.Single(s => s.State == "CA").AverageAge);
            Assert.AreEqual(expectedAverageAgeIL, groupingByState.Single(s => s.State == "IL").AverageAge);
        }

        
    }
}
