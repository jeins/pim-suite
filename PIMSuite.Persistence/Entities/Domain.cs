using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIMSuite.Persistence.Entities
{
    public class Domain
    {
        public Domain()
        {
            CreatedAt = DateTime.Now;
        }

        [Key]
        public int DomainId { get; set; }

        [Display(Name = "Domain")]
        [Required(ErrorMessage = "Domain ist erforderlich!")]
        public string DomainName { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
