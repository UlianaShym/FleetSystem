using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Globalization;
using FleetManagement.DataAccess.Entities;

namespace FleetManagement.DataAccess.DbContext
{
    public class ApplicationDbContext : System.Data.Entity.DbContext
    {
        public ApplicationDbContext()
            : base("FleetManaement")
        {
        }

        public DbSet<Car> Cars { get; set; }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}
