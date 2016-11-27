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
            CreatedAt = DateTime.Now;
        }
        
        [Key]
        public int MessageId { get; set; }
        
        public Guid SenderUserId { get; set; }
        
        public Guid ReceiverUserId { get; set; }

        public string MessageBody { get; set; }

        public bool IsRead { get; set; }

        public DateTime CreatedAt { get; set; }

    }
}
