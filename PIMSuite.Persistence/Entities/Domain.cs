using System;
using System.ComponentModel.DataAnnotations;

namespace PIMSuite.Persistence.Entities
{
    public class Domain
    {
        // Constructors

        public Domain()
        {
            CreatedAt = DateTime.Now;
        }

        // Properties

        [Key]
        public int DomainId { get; set; }

        [Display(Name = "Domain")]
        [Required(ErrorMessage = "Domain ist erforderlich!")]
        public string DomainName { get; set; }

        public DateTime CreatedAt { get; private set; }
    }
}