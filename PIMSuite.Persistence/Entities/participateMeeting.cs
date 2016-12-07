using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIMSuite.Persistence.Entities
{
    public class participateMeeting
    {
        public participateMeeting()
        {
            ParticipationId = Guid.NewGuid();
            Creation = DateTime.Now;
        }
        [Key]
        public Guid ParticipationId { get; set; }
        public Guid MeetingId { get; set; }
        public Guid UserId { get; set; }
        public DateTime Creation { get; set; }
    }
}
