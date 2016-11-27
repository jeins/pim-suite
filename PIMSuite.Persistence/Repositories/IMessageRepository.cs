using PIMSuite.Persistence.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIMSuite.Persistence.Repositories
{
    public interface IMessageRepository
    {
        IEnumerable<Message> GetConversationOfTwoUsers(Guid FirstUser, Guid SecondUser);
        
        void InsertMessage(Message m);

        void DeleteMessage (Guid MessageId);

        void Save();
    }
}
