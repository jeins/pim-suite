using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PIMSuite.Persistence.Entities;
using System.Data.Entity;
using System.Reflection;

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

        public User GetUserByUsername(String Username)
        {
            User myUser = context.Users.SingleOrDefault(user => user.Username == Username);
            return myUser;
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
            var entity = context.Users.SingleOrDefault(dbuser => dbuser.UserId == user.UserId);
            
            foreach (PropertyInfo propertyInfo in user.GetType().GetProperties())
            {
                if (propertyInfo.GetValue(user) == null)
                {
                    propertyInfo.SetValue(user, entity.GetType().GetProperty(propertyInfo.Name).GetValue(entity, null));
                }
            }
            user.isAdmin = entity.isAdmin;
            user.isValidated = entity.isValidated;
            user.Creation = entity.Creation;
            context.Entry(entity).CurrentValues.SetValues(user);
            context.Users.Attach(entity);
            context.Entry(entity).State = EntityState.Modified;
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
