using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PIMSuite.Persistence.Entities
{
    public class Event_Invite
    {
        // Constructors

        public Event_Invite()
        {
            CreatedAt = DateTime.Now;
        }

        // Properties

        [Key]
        public int InviteId { get; private set; }

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
        public int Status { get; set; }
        
        public DateTime CreatedAt { get; private set; }
    }
}