using System;
using System.Collections.Generic;
using System.Linq;
using PIMSuite.Persistence.Entities;
using System.Data.Entity;

namespace PIMSuite.Persistence.Repositories
{
    public class LeadershipRepository : ILeadershipRepository, IDisposable
    {
        // Constructors

        public LeadershipRepository(DataContext context)
        {
            _context = context;
        }

        // Fields

        private readonly DataContext _context;
        private bool _disposed = false;

        // Methods

        public void DeleteLeadership(Guid userId)
        {
            _context.Leaderships.Remove(_context.Leaderships.Find(userId));
        }
        
        public IEnumerable<Leadership> GetLeadership()
        {
            return _context.Leaderships.ToList();
        }

        public Leadership GetLeadershipByUserId(Guid userId)
        {
            return _context.Leaderships.Find(userId);
        }

        public void InsertLeadership(Leadership leadership)
        {
            _context.Leaderships.Add(leadership);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void UpdateLeadership(Leadership leadership)
        {
            _context.Entry(leadership).State = EntityState.Modified;
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