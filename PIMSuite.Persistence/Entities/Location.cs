using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIMSuite.Persistence.Entities
{
    public class Location
    {
        public Location()
        {
            Creation = DateTime.Now;
        }


        [Key]
        public string Name { get; set; }
        public DateTime Creation { get; set; }

        public ICollection<User> Users { get; set; }

    }
}
