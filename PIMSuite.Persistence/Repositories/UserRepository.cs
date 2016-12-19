using System;
using System.Collections.Generic;
using System.Linq;
using PIMSuite.Persistence.Entities;
using System.Data.Entity;
using System.Reflection;


namespace PIMSuite.Persistence.Repositories
{
    public class UserRepository : IUserRepository, IDisposable
    {
        // Constructors

        public UserRepository(DataContext dataContext)
        {
            _context = dataContext;
        }

        // Fields

        private readonly DataContext _context;
        private bool _disposed = false;

        // Methods

        public void DeleteUser(Guid guidId)
        {
            _context.Users.Remove(_context.Users.Find(guidId));
        }

        public User GetUserByID(Guid guidId)
        {
            return _context.Users.Find(guidId);
        }

        public User GetUserByUsername(String username)
        {
            User myUser = _context.Users.SingleOrDefault(user => user.Username == username);
            return myUser;
        }

        public IEnumerable<User> GetUsers()
        {
            return _context.Users.ToList();
        }

        public void InsertUser(User user)
        {
            _context.Users.Add(user);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void ValidateUser(User user, bool validated)
        {
            var entity = _context.Users.SingleOrDefault(dbuser => dbuser.UserId == user.UserId);
            entity.IsValidated = validated;
            _context.Users.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }

        public void UpdateUser(User user)
        {
            var entity = _context.Users.SingleOrDefault(dbuser => dbuser.UserId == user.UserId);
            
            foreach (PropertyInfo propertyInfo in user.GetType().GetProperties())
            {
                if (propertyInfo.GetValue(user) == null)
                {
                    propertyInfo.SetValue(user, entity.GetType().GetProperty(propertyInfo.Name).GetValue(entity, null));
                }
            }
            user.IsAdmin = entity.IsAdmin;
            user.IsValidated = entity.IsValidated;
            user.Modified = entity.Modified;
            _context.Entry(entity).CurrentValues.SetValues(user);
            _context.Users.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

     
    }
}