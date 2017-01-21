using FluentValidation.Attributes;
using PIMSuite.Persistence.Validators;
using System;
using System.ComponentModel.DataAnnotations;

namespace PIMSuite.Persistence.Entities
{
    [Validator(typeof(DomainValidator))]
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