using System.Linq;
using System.Collections.Generic;
using EFCoreWebApp.Core.Entities;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace EFCore5WebApp.DAL.Tests
{
    [TestFixture]
    public class UpdateTests
    {
        private AppDbContext _context;
        private int _personId;

        [SetUp]
        public void SetUp()
        {
            _context = new AppDbContext(
                new DbContextOptionsBuilder<AppDbContext>()
                    .UseSqlServer(
                        "Server=(localdb)\\mssqllocaldb;Database=EfCoreLearningWebApp;Trusted_Connection=True;MultipleActiveResultSets=true")
                    .Options
            );

            var record = new Person()
            {
                FirstName = "Clarke",
                LastName = "Kent",
                EmailAddress = "clark@daileybugel.com",
                Addresses = new List<Address>
                {
                    new Address
                    {
                        AddressLine1 = "1234 Fake Street",
                        AddressLine2 = "Suite 1",
                        City = "Chicago",
                        State = "IL",
                        ZipCode = "60652",
                        Country = "United States"
                    },
                }
            };
            
            _context.Persons.Add(record);
            _context.SaveChanges();
            _personId = record.Id;
        }
        
        [TearDown]
        public void TearDown()
        {
            var person = _context.Persons.Single(x => x.Id == _personId);
            _context.Persons.Remove(person);
            _context.SaveChanges();
        }

        [Test]
        public void UpdatePersonWithAddresses()
        {
            var person = _context.Persons.Single(x => x.Id == _personId);
            const string firstName = "Matt";
            const string lastName = "Smith";
            const string email = "doctor@who.com";
            person.FirstName = firstName;
            person.LastName = lastName;
            person.EmailAddress = email;
            var address = person.Addresses.First();
            const string addressLine1 = "123 Update St";
            const string addressLine2 = "APT B1";
            const string city = "Okemos";
            const string state = "MI";
            const string country = "USA";
            const string zipCode = "48864";
            address.AddressLine1 = addressLine1;
            address.AddressLine2 = addressLine2;
            address.City = city;
            address.State = state;
            address.Country = country;
            address.ZipCode = zipCode;
            _context.SaveChanges();
            
            var updatedPerson = _context.Persons.Single(x => x.Id == _personId);
            Assert.AreEqual(1, updatedPerson.Addresses.Count);
            Assert.AreEqual(firstName, updatedPerson.FirstName);
            Assert.AreEqual(lastName, updatedPerson.LastName);
            Assert.AreEqual(email, updatedPerson.EmailAddress);
            var updatedAddress = updatedPerson.Addresses.First();
            Assert.AreEqual(addressLine1, updatedAddress.AddressLine1);
            Assert.AreEqual(addressLine2, updatedAddress.AddressLine2);
            Assert.AreEqual(city, updatedAddress.City);
            Assert.AreEqual(state, updatedAddress.State);
            Assert.AreEqual(zipCode, updatedAddress.ZipCode);
            Assert.AreEqual(country, updatedAddress.Country);
        }
    }
}