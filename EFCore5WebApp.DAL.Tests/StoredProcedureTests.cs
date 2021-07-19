using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Linq;
using EFCoreWebApp.Core.Entities;

namespace EFCore5WebApp.DAL.Tests
{
    [TestFixture]
    public class StoredProcedureTests
    {
        private AppDbContext _context;
        [SetUp]
        public void SetUp()
        {
            _context = new AppDbContext(
                new DbContextOptionsBuilder<AppDbContext>()
                    .UseSqlServer(
                        "Server=(localdb)\\mssqllocaldb;Database=EfCoreLearningWebApp;Trusted_Connection=True;MultipleActiveResultSets=true")
                    .Options);
        }

        [Test]
        public void GetPersonsByStateInterpolated()
        {
            const int expectedNumberOfPeopleCA = 1;
            const string california = "CA";

            var californiaResidents = _context.Persons
                .FromSqlInterpolated($"GetPersonsByState {california}").ToList();
            
            Assert.AreEqual(expectedNumberOfPeopleCA, californiaResidents.Count);
        }

        [Test]
        public void GetPersonsByStateRaw()
        {
            const int expectedNumberOfPeopleIL = 2;
            const string illinois = "IL";

            var illinoisResidents = _context.Persons
                .FromSqlRaw($"GetPersonsByState @p0", new object[]{illinois}).ToList();

            Assert.AreEqual(expectedNumberOfPeopleIL, illinoisResidents.Count);
        }

        [Test]
        public void AddLookUpItemInterpolated()
        {
            const string code = "CAN";
            const string description = "Canada";
            const LookUpType lookUpType = LookUpType.Country;

            _context.Database.ExecuteSqlInterpolated($"AddLookUpItem {code}, {description}, {lookUpType}");

            var addedLookUpType = _context.LookUps.Single(x => x.Code == "CAN");

            Assert.IsNotNull(addedLookUpType);

            _context.LookUps.Remove(addedLookUpType);
            _context.SaveChanges();
        }

        [Test]
        public void AddLookUpItemRawSql()
        {
            const string code = "MEX";
            const string description = "Mexico";
            const LookUpType lookUpType = LookUpType.Country;

            _context.Database.ExecuteSqlRaw("AddLookUpItem @p0, @p1, @p2",
                new object[] {code, description, lookUpType}
            );

            var addedLookUpType = _context.LookUps.Single(x => x.Code == "MEX");

            Assert.IsNotNull(addedLookUpType);

            _context.Remove(addedLookUpType);
            _context.SaveChanges();
        }

    }
}
