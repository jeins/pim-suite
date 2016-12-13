using PIMSuite.Persistence.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIMSuite.Persistence.Repositories
{
    public interface ICalenderRepository
    {
        IEnumerable<Calendar> getAllPublicCalendarsByUserId(Guid UserId);
    }
}
