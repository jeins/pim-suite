using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PIMSuite.Persistence.Entities;
using PIMSuite.Persistence;
using System.Data.Entity;

namespace PIMSuite.Persistence.Repositories
{
    public class LocationRepository : ILocationRepository, IDisposable
    {
        private DataContext context;
        private bool disposed = false;


        public LocationRepository(DataContext context)
        {
            this.context=context;
        }

        public void DeleteLocation(string LocationName)
        {
            context.Locations.Remove(context.Locations.Find(LocationName));
        }
        
        public Location GetLocationByName(string LocationName)
        {
            return context.Locations.Find(LocationName);
        }

        public IEnumerable<Location> GetLocations()
        {
            return context.Locations.ToList();
        }

        public void InsertLocation(Location Location)
        {
            context.Locations.Add(Location);
        }

        public void UpdateLocation(Location Location)
        {
            context.Entry(Location).State = EntityState.Modified;
        }

        public void Save()
        {
            context.SaveChanges();
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
