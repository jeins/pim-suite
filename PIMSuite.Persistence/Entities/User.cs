using FluentValidation.Attributes;
using Newtonsoft.Json;
using PIMSuite.Persistence.Validators;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        }

        // Properties
        [Key]
        public Guid UserId { get; set; }

        [Required(ErrorMessage = "Vorname ist erforderlich!")]
        [Display(Name = "Vorname")]
        public string Firstname { get; set; }

        [Required(ErrorMessage = "Nachname ist erforderlich!")]
        [Display(Name = "Nachname")]
        public string Lastname { get; set; }

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

        [Required(ErrorMessage = "Passwort ist erforderlich!")]
        [DataType(DataType.Password)]
        [Display(Name = "Passwort")]
        [JsonIgnore]
        public string Password { get; set; }

        [Display(Name = "Administrator")]
        public Boolean isAdmin { get; set; }

        public DateTime Creation { get; set; }
               
        [Display(Name = "Validation Token")]
        [JsonIgnore]
        public string ValidationToken { get; set; }

        [Display(Name = "Validated")]
        [JsonIgnore]
        public Boolean isValidated { get; set; }

        public ICollection<Message> SentMessages { get; set; }
        public ICollection<Message> ReceivedMessages { get; set; }
        // TODO: remaining properties
    }
}
