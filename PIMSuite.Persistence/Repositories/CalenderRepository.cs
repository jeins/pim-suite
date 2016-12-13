using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PIMSuite.Persistence.Entities;

namespace PIMSuite.Persistence.Repositories
{
    public class CalenderRepository : ICalenderRepository
    {
        private DataContext context;
        public CalenderRepository(DataContext context)
        {
            this.context = context;
        }

        public IEnumerable<Calendar> getAllPublicCalendarsByUserId(Guid UserId)
        {
            return context.Calendars.Where(c => c.OwnerId == UserId && c.Privacy == false).ToList();
        }
    }
}
