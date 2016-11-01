using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIMSuite.Persistence.Entities
{
    public class User
    {
        // Constructors

        public User()
        {
            UserId = Guid.NewGuid();
        }

        // Properties

        public Guid UserId { get; set; }

        // TODO: remaining properties
    }
}
