using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PIMSuite.Persistence.Entities;

namespace PIMSuite.Persistence.Repositories
{
    public interface IMessageRepository
    {
        IEnumerable<string[]> GetMessageHistories(Guid senderUserGuid, Guid receiverUserGuid);
        void InsertMessage(Guid senderUserGuid, Guid receiverUserGuid, string messageBody);
    }
}
