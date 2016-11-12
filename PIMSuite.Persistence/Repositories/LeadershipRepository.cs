using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PIMSuite.Persistence.Entities;
using System.Data.Entity;

namespace PIMSuite.Persistence.Repositories
{
    public class LeadershipRepository : ILeadershipRepository, IDisposable
    {
        private DataContext context;
        private bool disposed = false;

        public LeadershipRepository(DataContext context)
        {
            this.context = context;
        }
        public void DeleteLeadership(Guid UserId)
        {
            context.Leaderships.Remove(context.Leaderships.Find(UserId));
        }

      

        public IEnumerable<Leadership> GetLeadership()
        {
            return context.Leaderships.ToList();
        }

        public Leadership GetLeadershipByUserId(Guid UserId)
        {
            return context.Leaderships.Find(UserId);
        }

        public void InsertLeadership(Leadership Leadership)
        {
            context.Leaderships.Add(Leadership);
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void UpdateLeadership(Leadership Leadership)
        {
            context.Entry(Leadership).State = EntityState.Modified;
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
