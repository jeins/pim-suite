using PIMSuite.Persistence.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIMSuite.Persistence.Repositories
{
    public interface ILeadershipRepository : IDisposable
    {

        IEnumerable<Leadership> GetLeadership();
        Leadership GetLeadershipByUserId(Guid GuidId);
        void InsertLeadership(Leadership Leadership);
        void DeleteLeadership(Guid UserId);
        void UpdateLeadership(Leadership Leadership);
        void Save();
    }
}
