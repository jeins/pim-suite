using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PIMSuite.Persistence.Entities
{
    public class Calendar
    {
        // Constructors

        public Calendar()
        {
            CreatedAt = DateTime.Now;
        }

        // Properties

        [Key]
        public int CalendarId { get; private set; }

        [ForeignKey("OwnerId")]
        public User Owner { get; set; }

        [Required(ErrorMessage = "Besitzer ist erforderlich!")]
        public Guid OwnerId { get; set; }

        [Required(ErrorMessage = "Name ist erforderlich!")]
        public string Name { get; set; }
        
        public bool IsPrivate { get; set; }

        public DateTime CreatedAt { get; private set; }
    }
}