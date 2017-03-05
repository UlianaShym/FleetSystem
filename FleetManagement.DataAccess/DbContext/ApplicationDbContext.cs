using System.Data.Entity;
using FleetManagement.DataAccess.Entities;

namespace FleetManagement.DataAccess.DbContext
{
    public class ApplicationDbContext : System.Data.Entity.DbContext
    {
        public ApplicationDbContext()
            : base("FleetManagement")
        {
        }

        public DbSet<Car> Cars { get; set; }
        public DbSet<Trip> Trips { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Country> Countries { get; set; }
    }
}
