using System;
using System.Collections.Generic;
using PIMSuite.Persistence.Entities;

namespace PIMSuite.Persistence.Repositories
{
    public interface IConnectionRepository
    {
        IEnumerable<Connection> GetConnectedUsers(Guid currentUserGuid);
        void InsertConnection(Guid userGuid, string connectionId);
        void UpdateConnection(Guid userGuid, string connectionId);
        Guid RemoveUser(Guid userGuid, string connectionId);
        void CleanUpConnection();
    }
}