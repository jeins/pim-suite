using PIMSuite.Persistence.Entities;
using System;
using System.Collections.Generic;

namespace PIMSuite.Persistence.Repositories
{
    public interface ICalendar_SubscriptionRepository
    {
        void Insert(Calendar_Subscription subscription);
        IEnumerable<Calendar_Subscription> GetAllSubscriptionsByUserId(Guid userId);
        void Delete(int calendarId, Guid subscriberId);
        Calendar_Subscription GetByUserIdAndByCalendarId(Guid subscriberId, int calendarId);
        bool SubscriptionContainsInUserList(int calendarId, Guid userId);
        void Save();                                        
    }
}
