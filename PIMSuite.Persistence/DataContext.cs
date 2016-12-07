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
        public DbSet<Meeting> Meetings { get; set; }
        public DbSet<participateMeeting> Participations { get; set; }

        //        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //        {
        //            //Database.SetInitializer(new CreateDatabaseIfNotExists<DbContext>());
        //            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<DbContext>());
        //            modelBuilder.Entity<Message>()
        //                       .HasRequired(m => m.Sender)
        //                       .WithMany(u => u.SentMessages)
        //                       .HasForeignKey(m => m.SenderId)
        //                       .WillCascadeOnDelete(false);
        //
        //            modelBuilder.Entity<Message>()
        //                       .HasRequired(m => m.Receiver)
        //                       .WithMany(u => u.ReceivedMessages)
        //                       .HasForeignKey(m => m.ReceiverId)
        //                       .WillCascadeOnDelete(false);
        //                       
        //            base.OnModelCreating(modelBuilder);
        //        }
    }
}
