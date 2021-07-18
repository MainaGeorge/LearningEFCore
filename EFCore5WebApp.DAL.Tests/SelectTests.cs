using System.Linq;
using EFCoreWebApp.Core.Entities;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace EFCore5WebApp.DAL.Tests
{
    [TestFixture]
    public class SelectTests
    {
        private AppDbContext _context;
        [SetUp]
        public void SetUp()
        {
            _context = new AppDbContext(
                new DbContextOptionsBuilder<AppDbContext>()
                    .UseSqlServer(
                        "Server=(localdb)\\mssqllocaldb;Database=EfCoreLearningWebApp;Trusted_Connection=True;MultipleActiveResultSets=true")
                    .Options
            );
        }

        [Test]
        public void GetAllPersons()
        {
            var persons = _context.Persons.ToList();
            Assert.AreEqual(2, persons.Count);
        }

        [Test]
        public void PersonsHaveAddresses()
        {
            var persons = _context.Persons
                .Include("Addresses")
                .ToList();

            Assert.AreEqual(1, persons[0].Addresses.Count);
            Assert.AreEqual(2, persons[1].Addresses.Count);
        }

        [Test]
        public void HaveLookUpAddresses()
        {
            var countries = _context.LookUps
                .Where(l => l.LookUpType == LookUpType.Country)
                .ToList();
            var states = _context.LookUps
                .Where(l => l.LookUpType == LookUpType.State)
                .ToList();

            Assert.AreEqual(1, countries.Count);
            Assert.AreEqual(51, states.Count);
        }
    }
}