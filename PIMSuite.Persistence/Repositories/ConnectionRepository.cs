using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PIMSuite.Persistence.Entities;

namespace PIMSuite.Persistence.Repositories
{
    public class ConnectionRepository : IConnectionRepository
    {
        private DataContext _dataContext;

        public ConnectionRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public IEnumerable<Connection> GetConnectedUsers(Guid currentUserGuid)
        {
            var users = _dataContext.Connections.Where(c => c.UserId != currentUserGuid);

            return users;
        }

        public void InsertConnection(Guid userGuid, string connectionId)
        {
            var connection = new Connection
            {
                ConnectionId = connectionId,
                UserId = userGuid
            };
            _dataContext.Connections.Add(connection);
            _dataContext.SaveChanges();
        }

        public void UpdateConnection(Guid userGuid, string connectionId)
        {
            var connection = _dataContext.Connections.SingleOrDefault(c => c.UserId.Equals(userGuid));
            if (connection != null)
            {
                connection.ConnectionId = connectionId;
                _dataContext.SaveChanges();
            }
        }

        public Guid RemoveUser(Guid userGuid, string connectionId)
        {
            if (userGuid.Equals(Guid.Empty))
            {
                var tmp = _dataContext.Connections.FirstOrDefault(c => c.ConnectionId == connectionId);
                if (tmp != null)
                {
                    userGuid = tmp.UserId;
                }
            }

            var connection = _dataContext.Connections.FirstOrDefault(c => c.ConnectionId == connectionId || c.UserId.Equals(userGuid));

            if (connection != null)
            {
                _dataContext.Connections.Remove(connection);
                _dataContext.SaveChanges();

                return userGuid;
            }

            return Guid.Empty;
        }

        public void CleanUpConnection()
        {
            throw new NotImplementedException();
        }
    }
}
