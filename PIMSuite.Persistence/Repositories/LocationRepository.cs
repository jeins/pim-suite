using System;
using System.Collections.Generic;
using System.Linq;
using PIMSuite.Persistence.Entities;
using System.Data.Entity;

namespace PIMSuite.Persistence.Repositories
{
    public class LocationRepository : ILocationRepository, IDisposable
    {
        // Constructors

        public LocationRepository(DataContext context)
        {
            _context = context;
        }

        // Fields

        private readonly DataContext _context;
        private bool _disposed = false;

        // Methods

        public void DeleteLocation(string locationName)
        {
            _context.Locations.Remove(_context.Locations.Find(locationName));
        }
        
        public Location GetLocationByName(string locationName)
        {
            return _context.Locations.Find(locationName);
        }

        public IEnumerable<Location> GetLocations()
        {
            return _context.Locations.ToList();
        }

        public void InsertLocation(Location location)
        {
            _context.Locations.Add(location);
        }

        public void UpdateLocation(Location location)
        {
            _context.Entry(location).State = EntityState.Modified;
        }

        public void Save()
        {
            _context.SaveChanges();
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