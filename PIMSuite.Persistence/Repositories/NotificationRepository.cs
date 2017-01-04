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

        public void ClearNotificationsForUser(Guid userId)
        {
            
        }
    }
}