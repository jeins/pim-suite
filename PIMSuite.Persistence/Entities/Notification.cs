using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace PIMSuite.Persistence.Entities
{
    public class Notification
    {
        // Constructors

        public Notification()
        {
            NotificationId = Guid.NewGuid();
            CreateDate = DateTime.UtcNow;
        }

        // Properties

        public Guid NotificationId { get; private set; }

        [ForeignKey("UserId")]
        public User User { get; set; }

        public Guid UserId { get; set; }

        public DateTime CreateDate { get; private set; }

        public string Message { get; set; }
    }
}