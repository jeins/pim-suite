using System;
using System.Collections.Generic;
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

        
        public string Description { get; set; } 

        //Achtung, ist als Freitext-String gedacht, daher keine Verknüpfung zu Location Entität
        public string Location { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime StartsAt { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime EndsAt { get; set; }

        //true ein privater Termin
        //false ein öffentlicher Termin
        //automatisch ein privater
        public bool IsPrivate { get; set; }
        
        //Für "Termin findet wirklich statt" Flag
        public bool Confirmed { get; set; }

        public DateTime CreatedAt { get; private set; }

        public virtual ICollection<Event_Invite> Invites { get; set; }
    }
}