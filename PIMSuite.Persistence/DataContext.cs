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
        public DataContext() : base("PIMDB")
        { }
        public DbSet<User> Users { get; set; }
        public DbSet<Leadership> Leaderships { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Connection> Connections { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //Database.SetInitializer(new CreateDatabaseIfNotExists<DbContext>());
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<DbContext>());

            base.OnModelCreating(modelBuilder);
        }
    }
}
