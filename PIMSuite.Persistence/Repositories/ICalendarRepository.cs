using PIMSuite.Persistence.Entities;
using System;
using System.Collections.Generic;

namespace PIMSuite.Persistence.Repositories
{
    public interface ICalendarRepository
    {
        IEnumerable<Calendar> GetAllCalendarsByUserId(Guid userId);
        IEnumerable<Calendar> GetAllUserCalendars(Guid userId);
        void InsertCalendar(Calendar calendar);
        void Save();
        Calendar GetCalendarByCalendarId(int calendarId);

    }
}