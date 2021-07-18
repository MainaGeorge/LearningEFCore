using EFCoreWebApp.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace EFCore5WebApp.DAL
{
    public class AppDbContext : DbContext
    {
        public DbSet<Person> Persons { get; set; }
        public DbSet<LookUp> LookUps { get; set; }
        public DbSet<Address> Addresses { get; set; }   
        public AppDbContext() : base()
        {
        }

        public AppDbContext(DbContextOptions options) : base(options)
        {
        }
    }
}
