using System;
using System.ComponentModel.DataAnnotations;

namespace PIMSuite.Persistence.Entities
{
    public class EventParticipation
    {
        // Constructors

        public EventParticipation()
        {
            PaticipationId = Guid.NewGuid();
            CreatedAt = DateTime.Now;
        }

        // Properties

        [Key]
        public Guid PaticipationId { get; private set; }

        public Guid CalendarEventId { get; set; }

        public Guid UserId { get; set; }

        public DateTime CreatedAt { get; private set; }
    }
}