using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PIMSuite.Persistence.Entities
{
    public class Department
    {
        // Constructors

        public Department()
        {
            Creation = DateTime.Now;   
        }

        // Properties
        
        [Key]
        public string Name {get; set;}

        public DateTime Creation {get; private set;}

        public ICollection<User> Users { get; set; }
    }
}