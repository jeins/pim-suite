using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIMSuite.Persistence.Entities
{
    public class EventParticipation
    {
        public EventParticipation()
        {
            PaticipationId = Guid.NewGuid();
            CreatedAt = DateTime.Now;
        }

        [Key]
        public Guid PaticipationId { get; set; }

        public Guid CalenderEventId { get; set; }

        public Guid UserId { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
