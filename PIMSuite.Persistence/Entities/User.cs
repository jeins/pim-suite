using FluentValidation.Attributes;
using Newtonsoft.Json;
using PIMSuite.Persistence.Validators;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PIMSuite.Persistence.Entities
{
    [Validator(typeof(UserValidator))]
    public class User
    {
        // Constructors

        public User()
        {
            UserId = Guid.NewGuid();
            Creation = DateTime.Now;
            Modified = Creation;
        }

        // Properties

        [Key]
        public Guid UserId { get; set; } // TODO: Should be private !!

        [Required(ErrorMessage = "Vorname ist erforderlich!")]
        [Display(Name = "Vorname")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Nachname ist erforderlich!")]
        [Display(Name = "Nachname")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Benutzername ist erforderlich!")]
        [Display(Name = "Benutzername")]
        [StringLength(20)]
        [Index("Username", 1, IsUnique = true)]
        public string Username { get; set; }

        [Required(ErrorMessage = "Email ist erforderlich!")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Abteilung ist erforderlich!")]
        [Display(Name = "Abteilung")]
        public string DepartmentName { get; set; }

        [ForeignKey("DepartmentName")]
        public Department Department { get; set; }

        [Required(ErrorMessage = "Telefonnumer ist erforderlich!")]
        [Display(Name = "Telefonnummer")]
        [DataType(DataType.PhoneNumber)]
        //[RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Bitte eine gültige Telefonnummer eingeben!")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Standort ist erforderlich!")]
        [Display(Name = "Standort")]
        public string LocationName { get; set; }
        [ForeignKey("LocationName")]
        public Location Location { get; set; }

        [Display(Name = "Projekte")]
        public string Projects { get; set; }

        [Required(ErrorMessage = "Passwort ist erforderlich!")]
        [DataType(DataType.Password)]
        [Display(Name = "Passwort")]
        [JsonIgnore]
        public string Password { get; set; }

        [Display(Name = "Administrator")]
        public bool IsAdmin { get; set; }

        public DateTime Creation { get; private set; }
               
        public DateTime Modified { get; set; }

        [Display(Name = "Validation Token")]
        [JsonIgnore]
        public string ValidationToken { get; set; }

        [Display(Name = "Validated")]
        [JsonIgnore]
        public bool IsValidated { get; set; }

        public ICollection<Message> SentMessages { get; set; }
        public ICollection<Message> ReceivedMessages { get; set; }

        public ICollection<Calendar> Calendars { get; set; }

        public ICollection<Calendar_Event> Events { get; set; }

        public ICollection<Calendar_Subscription> CalendarSubscriptions { get; set; }

        public ICollection<Event_Invite> SentInvites { get; set; }

        public ICollection<Event_Invite> ReceivedInvites { get; set; }
        // TODO: remaining properties
    }
}