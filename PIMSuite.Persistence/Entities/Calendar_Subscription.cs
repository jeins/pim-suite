using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIMSuite.Persistence.Entities
{
    public class Calendar_Subscription
    {
        public Calendar_Subscription()
        {
            CreatedAt = DateTime.Now;
        }

        [Key]
        public int SubscriptionId { get; set; }

        [Required(ErrorMessage = "Abonnent ist erforderlich!")]
        public Guid SubscriberId { get; set; }

        [ForeignKey("SubscriberId")]
        public User Subscriber { get; set; }

        [Required(ErrorMessage = "Kalender ist erforderlich!")]
        public int CalendarId { get; set; }

        [ForeignKey("CalendarId")]
        public Calendar Calendar { get; set; }

        public DateTime CreatedAt { get; set; }

    }
}
