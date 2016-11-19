using PIMSuite.Persistence.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIMSuite.Persistence.Repositories
{
    public interface IUserRepository : IDisposable
    {   
        IEnumerable<User> GetUsers();
        User GetUserByID(Guid GuidId);

        User GetUserByUsername(String Username);
        void InsertUser(User User);
        void DeleteUser(Guid GuidId);
        void UpdateUser(User user);
        void Save();
    }
}
