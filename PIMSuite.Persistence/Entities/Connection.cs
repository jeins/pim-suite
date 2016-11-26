using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIMSuite.Persistence.Entities
{
    public class Connection
    {
        public Connection()
        {       
            Id = Guid.NewGuid();
        }

        [Key]
        public Guid Id { get; set; }

        public string ConnectionId { get; set; }

        public Guid UserId { get; set; }

        public virtual User User { get; set; }
    }
}
