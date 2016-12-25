using System;
using System.ComponentModel.DataAnnotations;

namespace PIMSuite.Persistence.Entities
{
    public class ChatGroup
    {
        public ChatGroup()
        {
            GroupId = Guid.NewGuid();
            CreatedAt = DateTime.Now;
        }

        [Key]
        public Guid GroupId { get; set; }

        public string GroupName { get; set; }

        public Guid OwnerId { get; set; }

        public DateTime CreatedAt { get; private set; }

        public virtual User User { get; set; }
    }
}