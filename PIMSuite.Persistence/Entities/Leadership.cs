using System;
using System.ComponentModel.DataAnnotations;

namespace PIMSuite.Persistence.Entities
{
    public class Leadership
    {
        // Constructors

        public Leadership()
        {
            LeadershipId = Guid.NewGuid();
            Creation = DateTime.Now;
        }

        // Properties

        [Key]
        public Guid LeadershipId { get; private set; }
        
        public Guid UserId { get; set; }

        public string DepartmentName { get; set; }

        public bool Chief { get; set; }

        public DateTime Creation { get; private set; }
    }
}