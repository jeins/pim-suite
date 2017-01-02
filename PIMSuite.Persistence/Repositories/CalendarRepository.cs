using System;
using System.Collections.Generic;
using System.Linq;
using PIMSuite.Persistence.Entities;

namespace PIMSuite.Persistence.Repositories
{
    public class CalendarRepository : ICalendarRepository
    {
        // Constructors

        public CalendarRepository(DataContext context)
        {
            _context = context;
        }

        // Fields

        private readonly DataContext _context;
        
        // Methods

        public IEnumerable<Calendar> GetAllCalendarsByUserId(Guid userId)
        {
            return _context.Calendars.Where(c => c.OwnerId == userId).ToList();
        }

  /*      public IEnumerable<Calendar> GetAllPublicCalendarsByUserId(Guid userId)
        {
            return _context.Calendars.Where(c => c.OwnerId == userId && !c.isPrivate).ToList();
        } */

        public void InsertCalendar(Calendar calendar)
        {
            _context.Calendars.Add(calendar);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public Calendar GetCalendarByCalendarId(int calendarId)
        {
            return _context.Calendars.FirstOrDefault(c => c.CalendarId == calendarId);
        }

        public Guid GetUserByCalendarId(int calendarId)
        {
            return _context.Calendars.Find(calendarId).OwnerId;
        }
    }
}