using PIMSuite.Persistence.Entities;
using System;
using System.Collections.Generic;

namespace PIMSuite.Persistence.Repositories
{
    public interface ICalendarRepository
    {
        IEnumerable<Calendar> GetAllPublicCalendarsByUserId(Guid userId);
    }
}