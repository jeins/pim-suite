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
        public DbSet<Domain> Domains { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Connection> Connections { get; set; }
        public DbSet<Calendar> Calendars { get; set; }
        public DbSet<Calendar_Event> CalendarEvents { get; set; }
        public DbSet<Calendar_Subscription> CalendarSubscriptions { get; set; }
        public DbSet<Event_Invite> EventInvites { get; set; }
        public DbSet<ChatGroup> ChatGroups { get; set; }
        public DbSet<UserChatGroup> UserChatGroups { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //Database.SetInitializer(new CreateDatabaseIfNotExists<DbContext>());
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<DbContext>());
            //modelBuilder.Entity<Message>()
            //           .HasRequired(m => m.Sender)
            //           .WithMany(u => u.SentMessages)
            //           .HasForeignKey(m => m.SenderId)
            //           .WillCascadeOnDelete(false);

            //modelBuilder.Entity<Message>()
            //           .HasRequired(m => m.Receiver)
            //           .WithMany(u => u.ReceivedMessages)
            //           .HasForeignKey(m => m.ReceiverId)
            //           .WillCascadeOnDelete(false);

            modelBuilder.Entity<Calendar>()
                        .HasRequired(m => m.Owner)
                        .WithMany(m => m.Calendars)
                        .HasForeignKey(m => m.OwnerId)
                        .WillCascadeOnDelete(false);

            modelBuilder.Entity<Event_Invite>()
                        .HasRequired(m => m.InviteReceiver)
                        .WithMany(m => m.ReceivedInvites)
                        .HasForeignKey(m => m.InviteReceiverId)
                        .WillCascadeOnDelete(false);

            modelBuilder.Entity<Event_Invite>()
                        .HasRequired(m => m.InviteSender)
                        .WithMany(m => m.SentInvites)
                        .HasForeignKey(m => m.InviteSenderId)
                        .WillCascadeOnDelete(false);

            modelBuilder.Entity<Event_Invite>()
                        .HasRequired(m => m.InviteEvent)
                        .WithMany(m => m.Invites)
                        .HasForeignKey(m => m.InviteEventId)
                        .WillCascadeOnDelete(false);

            base.OnModelCreating(modelBuilder);
        }
    }
}
