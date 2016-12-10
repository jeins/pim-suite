using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIMSuite.Persistence.Entities
{
    public class Calendar_Event
    {
        public Calendar_Event()
        {
            CreatedAt = DateTime.Now;
        }

        [Key]
        public int EventId { get; set; }

        [Required(ErrorMessage = "Besitzer ist erforderlich!")]
        public Guid OwnerId { get; set; }

        [ForeignKey("OwnerId")]
        public User Owner { get; set; }

        [Required(ErrorMessage = "Kalender ist erforderlich!")]
        public int CalendarId { get; set; }

        [ForeignKey("CalendarId")]
        public Calendar Calendar { get; set; }

        [Required(ErrorMessage = "Name ist erforderlich!")]
        public string Name { get; set; }

        public string Description { get; set; } 

        //Achtung, ist als Freitext-String gedacht, daher keine Verknüpfung zu Location Entität
        public string Location { get; set; }

        //Für ganztagige Events
        public Boolean AllDayEvent { get; set; }

        //Für "Termin findet wirklich statt" Flag
        public Boolean Confirmed { get; set; }

        [Required(ErrorMessage = "Startzeit ist erforderlich!")]
        public DateTime StartTime { get; set; }

        [Required(ErrorMessage = "Endzeit ist erforderlich!")]
        public DateTime EndTime { get; set; }

        public DateTime CreatedAt { get; set; }


    }
}
