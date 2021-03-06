﻿using Newtonsoft.Json;
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

        [JsonIgnore]
        [ForeignKey("InviteSenderId")]
        public User InviteSender { get; set; }

        [Required(ErrorMessage = "Eingeladener ist erforderlich!")]
        public Guid InviteReceiverId { get; set; }

        [JsonIgnore]
        [ForeignKey("InviteReceiverId")]
        public User InviteReceiver { get; set; }

        [JsonProperty]
        public virtual User InviteReceiverUser
        {
            get
            {
                var dataContext = new DataContext();
                var user = dataContext.Users.Find(InviteReceiverId);
                return user;
            }
        }

        [Required(ErrorMessage = "Event ist erforderlich!")]
        public int InviteEventId { get; set; }

        [JsonIgnore]
        [ForeignKey("InviteEventId")]
        public Calendar_Event InviteEvent { get; set; }

        //0: Eingeladen, 1: Angenommen, 2: Abgelehnt
        [Required(ErrorMessage = "Status ist erforderlich!")]
        public int Status { get; set; }
        
        public DateTime CreatedAt { get; private set; }
    }
}