using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PIMSuite.Persistence.Entities;

namespace PIMSuite.Persistence.Repositories
{
    public interface IConnectionRepository
    {
        IEnumerable<Connection> GetConnectedUsers(Guid currentUserGuid);
        void InsertConnection(Guid userGuid, string connectionId);
        void UpdateConnection(Guid userGuid, string connectionId);
        void CleanUpConnection();
    }
}
