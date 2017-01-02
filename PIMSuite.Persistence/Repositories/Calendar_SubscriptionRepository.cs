using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PIMSuite.Persistence.Entities;
using System.Web;

namespace PIMSuite.Persistence.Repositories
{
    public class Calendar_SubscriptionRepository : ICalendar_SubscriptionRepository
    {
        // Constructors

        public Calendar_SubscriptionRepository (DataContext context)
        {
            _context = context;
        }

        // Fields

        private readonly DataContext _context;

        // Methods

        public void Delete(int CalendarId, Guid SubscriberId)
        {
           var subs= _context.CalendarSubscriptions.Where(s=>s.CalendarId==CalendarId && s.SubscriberId==SubscriberId);
            foreach(Calendar_Subscription sub in subs)
            {
                _context.CalendarSubscriptions.Remove(sub);
            }
        }

        public IEnumerable<Calendar_Subscription> getAllSubscriptionsByUserId(Guid UserId)
        {
            return _context.CalendarSubscriptions.Where(s=>s.SubscriberId==UserId).ToList();
        }

        public void Insert(Calendar_Subscription _subscription)
        {
            _context.CalendarSubscriptions.Add(_subscription);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public Calendar_Subscription getByUserIdAndByCalendarId(Guid subscriberId, int calendarId)
        {
            return _context.CalendarSubscriptions.Where(s => s.CalendarId == calendarId && s.SubscriberId == subscriberId).First(); 
        }

        

        bool ICalendar_SubscriptionRepository.SubscrptionContainsinUserList(int calendarId, Guid userId)
        {
            var subs = _context.CalendarSubscriptions.Where(s => s.SubscriberId == userId).ToList();
            var flag = false;
            foreach (Calendar_Subscription sub in subs)
            {
                if (sub.CalendarId == calendarId)
                {
                    flag = true;
                }
            }
            return flag;
        }
    }
}
