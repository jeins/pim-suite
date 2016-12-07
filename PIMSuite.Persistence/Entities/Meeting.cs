using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIMSuite.Persistence.Entities
{
    public class Meeting
    {
        public Meeting()
        {
            MeetingId = Guid.NewGuid();
            Creation = DateTime.Now;
        }

        [Key]
        public Guid MeetingId { get; set;}
        public String Privacy { get; set; }
        public String Titel { get; set; }
        public DateTime Date { get; set; }
        public DateTime Time { get; set; }
        public String Location { get; set; }
        public DateTime Duration { get; set; }
        public DateTime Creation { get; set; }
    }
}
