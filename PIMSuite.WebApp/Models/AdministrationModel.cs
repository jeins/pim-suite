using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PIMSuite.WebApp.Models
{
    public class AdministrationModel
    {
        public IEnumerable<PIMSuite.Persistence.Entities.User> Users{get;set;}
        public IEnumerable<PIMSuite.Persistence.Entities.Department> Departments { get; set; }
        public IEnumerable<PIMSuite.Persistence.Entities.Location> Locations { get; set; }
        public IEnumerable<PIMSuite.Persistence.Entities.Domain> Domains { get; set; }
    }
}