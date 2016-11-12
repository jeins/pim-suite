using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PIMSuite.Persistence.Entities;
using System.Data.Entity;

namespace PIMSuite.Persistence.Repositories
{
    public class UserRepository : IUserRepository, IDisposable
    {
        private DataContext context;
        private bool disposed = false;

        public UserRepository(DataContext dataContext)
        {
            this.context = dataContext;
        }

        public void DeleteUser(Guid GuidId)
        {
            context.Users.Remove(context.Users.Find(GuidId));
        }

        public User GetUserByID(Guid GuidId)
        {
            return context.Users.Find(GuidId);
        }

        public IEnumerable<User> GetUsers()
        {
            return context.Users.ToList();
        }

        public void InsertUser(User user)
        {
            context.Users.Add(user);
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void UpdateUser(User user)
        {
            context.Entry(user).State = EntityState.Modified;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
