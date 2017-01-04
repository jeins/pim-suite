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

        public void Delete(int calendarId, Guid subscriberId)
        {
           var subs = _context.CalendarSubscriptions.Where(s=>s.CalendarId == calendarId && s.SubscriberId == subscriberId);
            foreach(Calendar_Subscription sub in subs)
            {
                _context.CalendarSubscriptions.Remove(sub);
            }
        }

        public IEnumerable<Calendar_Subscription> GetAllSubscriptionsByUserId(Guid userId)
        {
            return _context.CalendarSubscriptions.Where(s=>s.SubscriberId == userId).ToList();
        }

        public void Insert(Calendar_Subscription subscription)
        {
            _context.CalendarSubscriptions.Add(subscription);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public Calendar_Subscription GetByUserIdAndByCalendarId(Guid subscriberId, int calendarId)
        {
            return _context.CalendarSubscriptions.Where(s => s.CalendarId == calendarId && s.SubscriberId == subscriberId).First(); 
        }
        
        public bool SubscriptionContainsInUserList(int calendarId, Guid userId)
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
