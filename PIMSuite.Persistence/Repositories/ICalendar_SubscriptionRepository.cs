using PIMSuite.Persistence.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIMSuite.Persistence.Repositories
{
    public interface ICalendar_SubscriptionRepository
    {

        void Insert(Calendar_Subscription _subscription);
        IEnumerable<Calendar_Subscription> getAllSubscriptionsByUserId(Guid UserId);
        void Delete(int CalendarId, Guid SubscriberId);
        Calendar_Subscription getByUserIdAndByCalendarId(Guid subscriberId, int calendarId);
        bool SubscrptionContainsinUserList(int calendarId, Guid userId);
        void Save();

                                                    
    }
}
