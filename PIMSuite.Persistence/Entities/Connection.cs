using System;
using System.ComponentModel.DataAnnotations;

namespace PIMSuite.Persistence.Entities
{
    public class Connection
    {
        // Constructors

        public Connection()
        {       
            Id = Guid.NewGuid();
        }

        // Properties

        [Key]
        public Guid Id { get; private set; }

        public string ConnectionId { get; set; }

        public Guid UserId { get; set; }

        public virtual User User { get; set; }
    }
}