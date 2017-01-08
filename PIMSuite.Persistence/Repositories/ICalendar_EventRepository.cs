using PIMSuite.Persistence.Entities;
using System;
using System.Collections.Generic;

namespace PIMSuite.Persistence.Repositories
{
    public interface ICalendar_EventRepository
    {
        void InsertCalendar_Event(Calendar_Event ev);
        void DeleteCalendar_Event(int eventId);
        Calendar_Event GetEvent(int eventId);
        IEnumerable<Calendar_Event> GetAllCalendar_EventByUserIdAndCalendarId(Guid userId, int calendarId);
        IEnumerable<Calendar_Event> GetInvites(Guid userId);
        void Save(); 
    }
}