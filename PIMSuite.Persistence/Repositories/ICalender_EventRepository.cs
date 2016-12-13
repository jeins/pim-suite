using PIMSuite.Persistence.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIMSuite.Persistence.Repositories
{
    public interface ICalender_EventRepository
    {
        void insertCalender_Event(Calendar_Event ev);
        void deleteCalender_Event(Guid eventId);
        IEnumerable<Calendar_Event> getAllCalender_EventByUserIdAndCalenderId(Guid UserId,int CaledarId);
        void Save(); 
    }
}
