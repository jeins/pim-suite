using System;
using System.Collections.Generic;
using System.Linq;
using PIMSuite.Persistence.Entities;

namespace PIMSuite.Persistence.Repositories
{
    public class Calendar_EventRepository : ICalendar_EventRepository
    {
        // Constructors

        public Calendar_EventRepository(DataContext context)
        {
            _context = context;
        }

        // Fields

        private readonly DataContext _context;

        // Methods

        public void DeleteCalendar_Event(int eventId)
        {
            _context.CalendarEvents.Remove(_context.CalendarEvents.SingleOrDefault(c => c.EventId == eventId));
        }

        public IEnumerable<Calendar_Event> GetAllCalendar_EventByUserIdAndCalendarId(Guid userId, int calendarId)
        {
            return _context.CalendarEvents.Where(c => c.CalendarId == calendarId && c.OwnerId == userId).ToList();
        }

        public IEnumerable<Calendar_Event> GetInvites(Guid userId)
        {
            return _context.CalendarEvents.Join(_context.EventInvites, e => e.EventId, ei => ei.InviteEventId, (e, ei) => new { e, ei }).Where(ce => ce.ei.InviteReceiverId == userId).Select(c => c.e);
        }

        public Calendar_Event GetEvent(int eventId)
        {
            return _context.CalendarEvents.SingleOrDefault(c => c.EventId == eventId);
        }

        public void InsertCalendar_Event(Calendar_Event ev)
        {
            _context.CalendarEvents.Add(ev);
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}