using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PIMSuite.Persistence.Entities;

namespace PIMSuite.Persistence.Repositories
{
    public class MessageRepository : IMessageRepository
    {

        private DataContext _dataContext;

        public MessageRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public IEnumerable<string[]> GetMessageHistories(Guid senderUserGuid, Guid receiverUserGuid)
        {
            var chatHistory = new List<string[]>();
            var messages =
                _dataContext.Messages
                .Where(m => m.ReceiverUserId.Equals(receiverUserGuid) && m.SenderUserId.Equals(senderUserGuid))
                .ToList();

            foreach (var message in messages)
            {
                var senderOrReceiverLabel = message.SenderUserId.Equals(senderUserGuid) ? "sender" : "receiver";
                var tmpArr = new[]
                {
                    message.SenderUserId.ToString(),
                    message.ReceiverUserId.ToString(),
                    message.MessageBody,
                    message.CreatedAt.ToString("g"),
                    senderOrReceiverLabel
                };

                chatHistory.Add(tmpArr);
            }

            return chatHistory;
        }

        public void InsertMessage(Guid senderUserGuid, Guid receiverUserGuid, string messageBody)
        {
            var message = new Message
            {
                ReceiverUserId = receiverUserGuid,
                SenderUserId = senderUserGuid,
                MessageBody = messageBody,
                IsRead = false
            };

            _dataContext.Messages.Add(message);
            _dataContext.SaveChanges();
        }
    }
}
