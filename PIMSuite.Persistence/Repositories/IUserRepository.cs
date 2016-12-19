using PIMSuite.Persistence.Entities;
using System;
using System.Collections.Generic;



namespace PIMSuite.Persistence.Repositories
{
    public interface IUserRepository : IDisposable
    {   
        IEnumerable<User> GetUsers();
        User GetUserByID(Guid guidId);
        User GetUserByUsername(String username);
        void InsertUser(User user);
        void DeleteUser(Guid guidId);
        void ValidateUser(User user, bool validated);
        void UpdateUser(User user);
        void Save();
    }
}