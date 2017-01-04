using System;
using System.Collections.Generic;
using PIMSuite.Persistence.Entities;

namespace PIMSuite.Persistence.Repositories
{
    public interface INotificationRepository
    {
        IList<Notification> GetNotificationsForUser(Guid userId);
        void ClearNotificationsForUser(Guid userId);
    }
}