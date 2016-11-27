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
        Guid InsertMessage(Guid senderUserGuid, Guid receiverUserGuid, string messageBody);
        void UpdateMessageStatus(Guid messageGuid, bool isRead);
        void SetMessageStatusToRead(Guid receiverUserGuid, Guid senderUserGuid);
        IEnumerable<string[]> GetUnReadMessages(Guid receiverUserGuid, Guid senderUserGuid);
    }
}
