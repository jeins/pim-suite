using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIMSuite.Persistence.Entities
{
    public class UserChatGroup
    {
        public UserChatGroup()
        {
            AddedAt = DateTime.Now;
        }

        [Key]
        public int Id { get; set; }

        public Guid GroupId { get; set; }

        public Guid UserId { get; set; }

        public DateTime AddedAt { get; private set; }

        public int NumUnReadMessage { get; set; }

        public virtual User User { get; set; }
        public virtual ChatGroup ChatGroup { get; set; }
    }
}
