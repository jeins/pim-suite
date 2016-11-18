using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIMSuite.Persistence.Entities
{
    public class Department
    {
        public Department()
        {
            Creation = DateTime.Now;
           
        }
        
        
        [Key]
        public string Name {get; set;}

        public DateTime Creation {get; set;}

        public ICollection<User> Users { get; set; }



    }
}
