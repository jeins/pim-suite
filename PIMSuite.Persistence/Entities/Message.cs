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
            isRead = false;
            Creation = DateTime.Now;
        }

        [Key]
        public Guid MessageId { get; set; }

        public Guid SenderId { get; set; }

        public Guid ReceiverId { get; set; }

        public String MessageBody { get; set; }

        public bool isRead { get; set; }

        public DateTime Creation { get; set; }

        [ForeignKey("SenderId")]
        public User Sender {get; set;}

        [ForeignKey("ReceiverId")]
        public User Receiver { get; set; }

       
    }
}
