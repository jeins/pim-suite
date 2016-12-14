using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PIMSuite.Persistence.Entities
{
    public class Calendar_Event
    {
        // Constructors

        public Calendar_Event()
        {
            CreatedAt = DateTime.Now;
            isPrivate = true;
        }

        // Properties

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

        [Required(ErrorMessage = "Titel der Veranstaltung ist erforderlich!")]
        public string Description { get; set; } 

        //Achtung, ist als Freitext-String gedacht, daher keine Verknüpfung zu Location Entität
        public string Location { get; set; }

        [Required(ErrorMessage = "Datum ist erforderlich!")]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "Uhrzeit ist erforderlich!")]
        public DateTime Time { get; set; }
        
        [Required(ErrorMessage = "Dauer ist erforderlich!")]
        public DateTime Duration { get; set; } 
        //true ein privater Termin
        //false ein öffentlicher Termin
        //automatisch ein privater
        public bool isPrivate { get; set; }
        
        //Für "Termin findet wirklich statt" Flag
        public bool Confirmed { get; set; }

        public DateTime CreatedAt { get; private set; }
    }
}