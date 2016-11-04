using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace DataPersistence
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Vorname ist erforderlich!")]
        public string Vorname { get; set; }
        [Required(ErrorMessage = "Nachname ist erforderlich!")]
        public string Nachname { get; set; }
        [Required(ErrorMessage = "Username ist erforderlich!")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Email ist erforderlich!")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "Abteilung ist erforderlich!")]
        public string Abteilung { get; set; }
        [Required(ErrorMessage = "Passwort ist erforderlich!")]
        [DataType(DataType.Password)]
        public string Passwort { get; set; }
    }
}
