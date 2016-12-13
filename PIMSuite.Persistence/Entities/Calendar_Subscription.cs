using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PIMSuite.Persistence.Entities
{
    public class Calendar_Subscription
    {
        // Constructors

        public Calendar_Subscription()
        {
            CreatedAt = DateTime.Now;
        }

        // Properties

        [Key]
        public int SubscriptionId { get; private set; }

        [Required(ErrorMessage = "Abonnent ist erforderlich!")]
        public Guid SubscriberId { get; set; }

        [ForeignKey("SubscriberId")]
        public User Subscriber { get; set; }

        [Required(ErrorMessage = "Kalender ist erforderlich!")]
        public int CalendarId { get; set; }

        [ForeignKey("CalendarId")]
        public Calendar Calendar { get; set; }

        public DateTime CreatedAt { get; private set; }
    }
}