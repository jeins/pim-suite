using PIMSuite.Persistence.Entities;
using System;
using System.Collections.Generic;

namespace PIMSuite.Persistence.Repositories
{
    public interface ILocationRepository : IDisposable
    {
        IEnumerable<Location> GetLocations();
        Location GetLocationByName(string locationName);
        void InsertLocation(Location location);
        void DeleteLocation(string locationName);
        void UpdateLocation(Location location);
        void Save();
    }
}