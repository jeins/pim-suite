using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIMSuite.Persistence.Entities
{
    public class Leadership
    {
        public Leadership()
        {
            LeadershipId = Guid.NewGuid();
            Creation = DateTime.Now;
        }

        // Properties
        [Key]
        public Guid LeadershipId { get; set; }
        
        public Guid UserId { get; set; }

        public string DepartmentName { get; set; }

        public bool Chief { get; set; }

        public DateTime Creation { get; set; }
    }
}
