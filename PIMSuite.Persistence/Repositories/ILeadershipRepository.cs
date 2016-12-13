using PIMSuite.Persistence.Entities;
using System;
using System.Collections.Generic;

namespace PIMSuite.Persistence.Repositories
{
    public interface ILeadershipRepository : IDisposable
    {
        IEnumerable<Leadership> GetLeadership();
        Leadership GetLeadershipByUserId(Guid guidId);
        void InsertLeadership(Leadership leadership);
        void DeleteLeadership(Guid userId);
        void UpdateLeadership(Leadership leadership);
        void Save();
    }
}