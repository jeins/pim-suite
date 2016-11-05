using PIMSuite.Persistence.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIMSuite.Persistence
{
    public class DataContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer(new CreateDatabaseIfNotExists<DbContext>());

            base.OnModelCreating(modelBuilder);
        }
    }
}
