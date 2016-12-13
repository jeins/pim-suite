using PIMSuite.Persistence.Entities;
using System;
using System.Collections.Generic;

namespace PIMSuite.Persistence.Repositories
{
    public interface ICalendar_EventRepository
    {
        void InsertCalendar_Event(Calendar_Event ev);
        void DeleteCalendar_Event(Guid eventId);
        IEnumerable<Calendar_Event> GetAllCalendar_EventByUserIdAndCalendarId(Guid userId, int calendarId);
        void Save(); 
    }
}