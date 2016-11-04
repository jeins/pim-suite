using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PIMSuite.WebApp.Models
{
    public class UserModel
    {
        public Guid UserId { get; set; }
      
        public string Vorname { get; set; }
       
        public string Nachname { get; set; }
      
        public string Username { get; set; }
       
        public string Email { get; set; }
    
        public string Abteilung { get; set; }
      
        public string Passwort { get; set; }
    }
}