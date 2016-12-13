using System;
using System.Collections.Generic;
using System.Linq;
using PIMSuite.Persistence.Entities;

namespace PIMSuite.Persistence.Repositories
{
    public class Calender_EventRepository : ICalender_EventRepository
    {
        private DataContext context;
        public Calender_EventRepository(DataContext context)
        {
            this.context = context;
        }
        public void deleteCalender_Event(Guid eventId)
        {
            context.CalendarEvents.Remove(context.CalendarEvents.Find(eventId));
        }

        public IEnumerable<Calendar_Event> getAllCalender_EventByUserIdAndCalenderId(Guid UserId, int CaledarId)
        {
            return context.CalendarEvents.Where(c => c.CalendarId == CaledarId  &&  c.OwnerId == UserId).ToList();
        }

        public void insertCalender_Event(Calendar_Event ev)
        {
            context.CalendarEvents.Add(ev);
        }

        public void Save()
        {
            context.SaveChanges();
        }
    }
}
