using PIMSuite.Persistence.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIMSuite.Persistence.Repositories
{
    public interface ILocationRepository : IDisposable
    {

        IEnumerable<Location> GetLocations();
        Location GetLocationByName(string LocationName);
        void InsertLocation(Location Location);
        void DeleteLocation(string LocationName);
        void UpdateLocation(Location Location);
        void Save();
    }
}
