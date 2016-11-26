using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIMSuite.Persistence.Entities
{
    public class Message
    {
        public Message()
        {
            MessageId = Guid.NewGuid();
            CreatedAt = DateTime.Now;
        }

        [Key]
        public Guid MessageId { get; set; }
        
        [ForeignKey("User")]
        public Guid SenderUserId { get; set; }

        [ForeignKey("User")]
        public Guid ReceiverUserId { get; set; }

        public string MessageBody { get; set; }

        public bool IsRead { get; set; }

        public DateTime CreatedAt { get; set; }

    }
}
