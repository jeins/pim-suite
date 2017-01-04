using System;
using System.Collections.Generic;
using System.Linq;
using PIMSuite.Persistence.Entities;

namespace PIMSuite.Persistence.Repositories
{
    public class NotificationRepository : INotificationRepository
    {        
        // Constructors

        public NotificationRepository(DataContext dataContext)
        {
            _context = dataContext;
        }

        // Fields

        private readonly DataContext _context;

        // Methods

        public IList<Notification> GetNotificationsForUser(Guid userId)
        {
            return _context.Notifications.Where(x => x.UserId == userId).OrderByDescending(x => x.CreateDate).ToList();
        }

        public void InsertNotification(Notification notification)
        {
            _context.Notifications.Add(notification);
        }

        public void ClearNotificationsForUser(Guid userId)
        {
            var notifications = _context.Notifications.Where(n => n.UserId == userId).ToList();
            foreach (var notification in notifications)
            {
                _context.Notifications.Remove(notification);
            }
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}