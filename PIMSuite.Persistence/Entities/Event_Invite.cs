using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIMSuite.Persistence.Entities
{
    public class Event_Invite
    {
        public Event_Invite()
        {
            CreatedAt = DateTime.Now;
        }

        [Key]
        public int InviteId { get; set; }

        [Required(ErrorMessage = "Einladender ist erforderlich!")]
        public Guid InviteSenderId { get; set; }

        [ForeignKey("InviteSenderId")]
        public User InviteSender { get; set; }

        [Required(ErrorMessage = "Eingeladener ist erforderlich!")]
        public Guid InviteReceiverId { get; set; }

        [ForeignKey("InviteReceiverId")]
        public User InviteReceiver { get; set; }

        //0: Eingeladen, 1: Angenommen, 2: Abgelehnt
        [Required(ErrorMessage = "Status ist erforderlich!")]
        public int status { get; set; }


        public DateTime CreatedAt { get; set; }

    }
}
