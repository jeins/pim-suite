using PIMSuite.Persistence.Entities;
using System;
using System.Collections.Generic;

namespace PIMSuite.Persistence.Repositories
{
    public interface ICalendar_EventRepository
    {
        void InsertCalender_Event(Calendar_Event ev);
        void DeleteCalender_Event(Guid eventId);
        IEnumerable<Calendar_Event> GetAllCalender_EventByUserIdAndCalenderId(Guid userId, int caledarId);
        void Save(); 
    }
}