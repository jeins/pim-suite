using System;
using System.Collections.Generic;
using System.Linq;
using PIMSuite.Persistence.Entities;

namespace PIMSuite.Persistence.Repositories
{
    public class Calender_EventRepository : ICalendar_EventRepository
    {
        // Constructors

        public Calender_EventRepository(DataContext context)
        {
            _context = context;
        }

        // Fields

        private readonly DataContext _context;

        // Methods

        public void DeleteCalender_Event(Guid eventId)
        {
            _context.CalendarEvents.Remove(_context.CalendarEvents.Find(eventId));
        }

        public IEnumerable<Calendar_Event> GetAllCalender_EventByUserIdAndCalenderId(Guid userId, int caledarId)
        {
            return _context.CalendarEvents.Where(c => c.CalendarId == caledarId  &&  c.OwnerId == userId).ToList();
        }

        public void InsertCalender_Event(Calendar_Event ev)
        {
            _context.CalendarEvents.Add(ev);
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}