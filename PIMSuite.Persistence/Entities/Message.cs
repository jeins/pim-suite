using System;
using System.ComponentModel.DataAnnotations;

namespace PIMSuite.Persistence.Entities
{
    public class Message
    {
        // Constructors

        public Message()
        {
            CreatedAt = DateTime.Now;
        }
        
        // Properties

        [Key]
        public int MessageId { get; private set; }
        
        public Guid SenderUserId { get; set; }
        
        public Guid ReceiverUserId { get; set; }

        public string MessageBody { get; set; }

        public bool IsRead { get; set; }

        public DateTime CreatedAt { get; private set; }
    }
}